using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiora.Data;
using Fiora.Models;

namespace Fiora.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VistaAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VistaAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VistaAdmin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            // Get statistics for dashboard
            ViewBag.TotalPedidos = await _context.Pedido.CountAsync();
            ViewBag.PedidosPendientes = await _context.Pedido
                .CountAsync(p => p.EstadoPedido == EstadoPedido.Pendiente);
            ViewBag.PedidosEnProceso = await _context.Pedido
                .CountAsync(p => p.EstadoPedido == EstadoPedido.EnProceso);
            ViewBag.TotalClientes = await _context.Cliente.CountAsync();

            return View();
        }

        // GET: VistaAdmin/Entregas
        public async Task<IActionResult> Entregas()
        {
            var pedidos = await _context.Pedido
                .Include(p => p.Cliente)
                .Include(p => p.Arreglo)
                .Where(p => p.EstadoPedido == EstadoPedido.Pendiente || 
                           p.EstadoPedido == EstadoPedido.EnProceso)
                .OrderBy(p => p.FechaHoraEntrega)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: VistaAdmin/Inventario
        public async Task<IActionResult> Inventario()
        {
            var inventario = await _context.Inventario
                .OrderBy(i => i.NombreProducto)
                .ToListAsync();

            return View(inventario);
        }

        // GET: VistaAdmin/Reportes
        public async Task<IActionResult> Reportes()
        {
            var reportes = await _context.Reporte
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            return View(reportes);
        }

        // GET: VistaAdmin/Clientes
        public async Task<IActionResult> Clientes()
        {
            var clientes = await _context.Cliente
                .Include(c => c.Pedidos)
                .OrderBy(c => c.NombreCliente)
                .ToListAsync();

            return View(clientes);
        }
    }
}
