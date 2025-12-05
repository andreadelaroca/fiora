using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Fiora.Controllers
{
    public class GoController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public GoController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Universal entry: sends user to the right place
        [HttpGet]
        [AllowAnonymous]
        [Route("MiVista")]
        public async Task<IActionResult> MiVista()
        {
            // If not signed-in, go to Identity Login
            if (!_signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    return Redirect("/VistaAdmin/DashboardStatic");
                }
                if (roles.Contains("Cliente"))
                {
                    return Redirect("/VistaUsuario/InicioStatic");
                }
            }

            // Fallback
            return Redirect("/");
        }
    }
}
