using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiora.Data;
using Fiora.Models;
using Microsoft.AspNetCore.Identity;

namespace Fiora.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class VistaUsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VistaUsuarioController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VistaUsuario/Inicio
        public IActionResult Inicio()
        {
            return View();
        }

        // GET: VistaUsuario/InicioStatic (versión estática sin dependencias de BD)
        public IActionResult InicioStatic()
        {
            return View("InicioStatic");
        }

        // GET: VistaUsuario/MisPedidos
        public async Task<IActionResult> MisPedidos()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Find cliente by email
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.CorreoCliente == user.Email);

            if (cliente == null)
                return RedirectToAction("Inicio");

            var pedidosActuales = await _context.Pedido
                .Include(p => p.Arreglo)
                .Where(p => p.ClienteId == cliente.Id && 
                           (p.EstadoPedido == EstadoPedido.Pendiente || 
                            p.EstadoPedido == EstadoPedido.EnProceso))
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(pedidosActuales);
        }

        // GET: VistaUsuario/NuevoPedido
        public async Task<IActionResult> NuevoPedido()
        {
            ViewBag.Arreglos = await _context.Arreglo
                .Where(a => a.Disponible)
                .ToListAsync();

            return View();
        }

        // GET: VistaUsuario/Historial
        public async Task<IActionResult> Historial()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.CorreoCliente == user.Email);

            if (cliente == null)
                return RedirectToAction("Inicio");

            var historialPedidos = await _context.Pedido
                .Include(p => p.Arreglo)
                .Where(p => p.ClienteId == cliente.Id && 
                           (p.EstadoPedido == EstadoPedido.Entregado || 
                            p.EstadoPedido == EstadoPedido.Cancelado))
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(historialPedidos);
        }

        // GET: VistaUsuario/Notificaciones
        public IActionResult Notificaciones()
        {
            return View();
        }
    }
}
