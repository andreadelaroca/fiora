using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fiora.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            // If already signed in, auto-redirect by role
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                        return LocalRedirect("/VistaAdmin/DashboardStatic");
                    if (roles.Contains("Cliente"))
                        return LocalRedirect("/VistaUsuario/InicioStatic");
                }
                return LocalRedirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Hardcode: if the seeded client account logs in, ensure role and go to client view
                if (string.Equals(Input.Email, "cliente@fiora.local", StringComparison.OrdinalIgnoreCase))
                {
                    var clientUser = await _userManager.FindByEmailAsync(Input.Email) ?? await _userManager.FindByNameAsync(Input.Email);
                    if (clientUser != null && !await _userManager.IsInRoleAsync(clientUser, "Cliente"))
                    {
                        await _userManager.AddToRoleAsync(clientUser, "Cliente");
                    }
                    return LocalRedirect("/VistaUsuario/InicioStatic");
                }

                // Otherwise, redirect by role
                var user = await _userManager.FindByEmailAsync(Input.Email) ?? await _userManager.FindByNameAsync(Input.Email);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                        return LocalRedirect("/VistaAdmin/DashboardStatic");
                    if (roles.Contains("Cliente"))
                        return LocalRedirect("/VistaUsuario/InicioStatic");
                }

                // Default fallback
                return LocalRedirect("/");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}