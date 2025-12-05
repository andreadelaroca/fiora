using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fiora.Data;
using Fiora.Models;

namespace Fiora.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "{0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            // Campos adicionales para Cliente
            [Required]
            [MaxLength(100)]
            public string NombreCliente { get; set; } = string.Empty;

            [Required]
            [MaxLength(20)]
            public string TelefonoCliente { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string DireccionCliente { get; set; } = string.Empty;

            // Selección explícita del tipo de usuario
            [Required]
            public string TipoUsuario { get; set; } = "Cliente"; // "Admin" o "Cliente"
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("/");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                // Auto sign-in after register
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (string.Equals(Input.TipoUsuario, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return LocalRedirect("/VistaAdmin/DashboardStatic");
                }
                else
                {
                    try
                    {
                        var cliente = new Cliente
                        {
                            NombreCliente = Input.NombreCliente,
                            CorreoCliente = Input.Email,
                            PasswordCliente = Input.Password,
                            TelefonoCliente = Input.TelefonoCliente,
                            DireccionCliente = Input.DireccionCliente,
                            FechaRegistroCliente = DateTime.UtcNow,
                            Estado = EstadoCliente.Activo
                        };
                        _db.Cliente.Add(cliente);
                        await _db.SaveChangesAsync();
                        return LocalRedirect(ReturnUrl ?? "/");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error guardando cliente: {ex.Message}");
                        return Page();
                    }
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}
