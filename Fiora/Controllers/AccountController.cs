using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiora.Data;
using Fiora.Models;

namespace Fiora.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "El correo y la contraseña son requeridos.");
                return View();
            }

            // Find user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View();
            }

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Check user roles and redirect accordingly
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Dashboard", "VistaAdmin");
                }
                else if (roles.Contains("Cliente"))
                {
                    return RedirectToAction("Inicio", "VistaUsuario");
                }
                else
                {
                    // Default redirect if no role assigned
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, string role = "Cliente")
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son requeridos.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Assign role
                await _userManager.AddToRoleAsync(user, role);

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect based on role
                if (role == "Admin")
                {
                    return RedirectToAction("Dashboard", "VistaAdmin");
                }
                else
                {
                    return RedirectToAction("Inicio", "VistaUsuario");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // Registro específico de Administrador (crea usuario de Identity y la fila en tabla Admin)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(string NombreAdmin, string CorreoAdmin, string PasswordAdmin, string PasswordConfirm, bool termsAccepted, string Estado = "Activo")
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(NombreAdmin) || string.IsNullOrWhiteSpace(CorreoAdmin) || string.IsNullOrWhiteSpace(PasswordAdmin))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son requeridos.");
                return View("Register");
            }

            if (PasswordAdmin != PasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View("Register");
            }

            if (!termsAccepted)
            {
                ModelState.AddModelError(string.Empty, "Debes aceptar los términos y condiciones de uso.");
                return View("Register");
            }

            // Email válido
            try
            {
                var addr = new System.Net.Mail.MailAddress(CorreoAdmin);
                if (addr.Address != CorreoAdmin)
                {
                    ModelState.AddModelError(string.Empty, "Correo electrónico inválido.");
                    return View("Register");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Correo electrónico inválido.");
                return View("Register");
            }

            // Asegurar rol Admin existe
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Crear usuario Identity
            var identityUser = new IdentityUser { UserName = CorreoAdmin, Email = CorreoAdmin };
            var createResult = await _userManager.CreateAsync(identityUser, PasswordAdmin);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Register");
            }

            // Asignar rol Admin
            await _userManager.AddToRoleAsync(identityUser, "Admin");

            // Mapear y guardar en tabla Admin
            var adminEntity = new Admin
            {
                NombreAdmin = NombreAdmin,
                CorreoAdmin = CorreoAdmin,
                PasswordAdmin = PasswordAdmin,
                Estado = EstadoAdmin.Activo
            };

            _db.Admin.Add(adminEntity);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Surface DB errors to the form instead of generic error page
                ModelState.AddModelError(string.Empty, $"Error al guardar el administrador: {ex.InnerException?.Message ?? ex.Message}");
                return View("Register");
            }

            // NO iniciar sesión automáticamente: llevar al inicio de sesión
            // para que luego al iniciar, se redirija al Dashboard por su rol Admin
            return RedirectToAction("Login", "Account");
        }

        // Registro específico de Cliente (crea usuario de Identity y la fila en tabla Cliente)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCliente(string NombreCliente, string CorreoCliente, string PasswordCliente, string PasswordConfirm, string TelefonoCliente, string DireccionCliente, bool termsAccepted, string Estado = "Activo")
        {
            if (string.IsNullOrWhiteSpace(NombreCliente) || string.IsNullOrWhiteSpace(CorreoCliente) || string.IsNullOrWhiteSpace(PasswordCliente) || string.IsNullOrWhiteSpace(TelefonoCliente) || string.IsNullOrWhiteSpace(DireccionCliente))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son requeridos.");
                return View("Register");
            }

            if (PasswordCliente != PasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View("Register");
            }

            if (!termsAccepted)
            {
                ModelState.AddModelError(string.Empty, "Debes aceptar los términos y condiciones de uso.");
                return View("Register");
            }

            // Email válido
            try
            {
                var addr = new System.Net.Mail.MailAddress(CorreoCliente);
                if (addr.Address != CorreoCliente)
                {
                    ModelState.AddModelError(string.Empty, "Correo electrónico inválido.");
                    return View("Register");
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Correo electrónico inválido.");
                return View("Register");
            }

            // Teléfono de 8 dígitos
            if (TelefonoCliente == null || TelefonoCliente.Length != 8 || !TelefonoCliente.All(char.IsDigit))
            {
                ModelState.AddModelError(string.Empty, "El teléfono debe tener 8 dígitos.");
                return View("Register");
            }

            // Asegurar rol Cliente existe
            if (!await _roleManager.RoleExistsAsync("Cliente"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Cliente"));
            }

            // Crear usuario Identity
            var identityUser = new IdentityUser { UserName = CorreoCliente, Email = CorreoCliente };
            var createResult = await _userManager.CreateAsync(identityUser, PasswordCliente);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Register");
            }

            await _userManager.AddToRoleAsync(identityUser, "Cliente");

            var clienteEntity = new Cliente
            {
                NombreCliente = NombreCliente,
                CorreoCliente = CorreoCliente,
                PasswordCliente = PasswordCliente,
                TelefonoCliente = TelefonoCliente,
                DireccionCliente = DireccionCliente,
                Estado = EstadoCliente.Activo
            };

            _db.Cliente.Add(clienteEntity);
            await _db.SaveChangesAsync();

            await _signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToAction("Inicio", "VistaUsuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Diagnóstico simple: verificar cuentas guardadas
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DebugAccounts()
        {
            var adminCount = _db.Admin.Count();
            var clienteCount = _db.Cliente.Count();
            return Json(new { admins = adminCount, clientes = clienteCount });
        }
    }
}
