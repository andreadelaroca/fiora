using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiora.Data;
using Fiora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var totals = new DashboardTotals
            {
                ClientesTotal = await _context.Cliente.CountAsync(),
                InventarioItems = await _context.Inventario.CountAsync(),
                PedidosActivos = await _context.Pedido.CountAsync(p => p.EstadoPedido == EstadoPedido.EnProceso || p.EstadoPedido == EstadoPedido.Pendiente),
                EntregasHoy = await _context.Pedido.CountAsync(p => p.FechaHoraEntrega != null && p.FechaHoraEntrega.Value.Date == DateTime.UtcNow.Date),
                VentasMes = 0m
            };

            var model = new DashboardViewModel
            {
                Totals = totals,
                RecentClients = await _context.Cliente
                    .OrderByDescending(c => c.Id)
                    .Take(6)
                    .ToListAsync(),
                LowStockItems = await _context.Inventario
                    .Where(i => i.Cantidad <= 5)
                    .OrderBy(i => i.Cantidad)
                    .Take(6)
                    .ToListAsync(),
                UpcomingDeliveries = await _context.Pedido
                    .Where(p => p.FechaHoraEntrega != null)
                    .OrderBy(p => p.FechaHoraEntrega)
                    .Take(6)
                    .ToListAsync(),
                Notifications = new List<NotificationItem>
                {
                    new NotificationItem { Fecha = DateTime.UtcNow.AddMinutes(-30), Titulo = "Nuevo cliente", Detalle = "Se registró un nuevo cliente." },
                    new NotificationItem { Fecha = DateTime.UtcNow.AddHours(-2), Titulo = "Pedido en proceso", Detalle = "El pedido #102 pasó a 'En Proceso'." },
                    new NotificationItem { Fecha = DateTime.UtcNow.AddDays(-1), Titulo = "Inventario bajo", Detalle = "Rosas rojas por debajo del umbral." }
                }
            };

            return View(model);
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

        [HttpPost]
        public async Task<IActionResult> CambiarEstadoEntrega(int id, EstadoPedido estado)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null) return NotFound();
            pedido.EstadoPedido = estado;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Entregas));
        }

        // GET: VistaAdmin/Inventario
        public async Task<IActionResult> Inventario()
        {
            var inventario = await _context.Inventario
                .OrderBy(i => i.NombreProducto)
                .ToListAsync();

            return View(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarInventarioCantidad(int id, int cantidad)
        {
            var item = await _context.Inventario.FindAsync(id);
            if (item == null) return NotFound();
            if (cantidad < 0) return BadRequest("Cantidad inválida");
            item.Cantidad = cantidad;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Inventario));
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

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null) return NotFound();
            cliente.Estado = cliente.Estado == EstadoCliente.Activo ? EstadoCliente.Inactivo : EstadoCliente.Activo;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Clientes));
        }
    }
}
