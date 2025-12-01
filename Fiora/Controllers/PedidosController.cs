using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fiora.Data;
using Fiora.Models;

namespace Fiora.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pedido.Include(p => p.Admin).Include(p => p.Arreglo).Include(p => p.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Admin)
                .Include(p => p.Arreglo)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Admin, "Id", "CorreoAdmin");
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo");
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "CorreoCliente");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OcasionPedido,ClienteId,NombreCliente,DireccionEnvio,MensajePedido,ModoPago,MontoTotal,EstadoPedido,Servicio,FechaHoraEntrega,DireccionEvento,TematicaEvento,ColoresEvento,AdminId,ArregloId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Admin, "Id", "CorreoAdmin", pedido.AdminId);
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", pedido.ArregloId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "CorreoCliente", pedido.ClienteId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Admin, "Id", "CorreoAdmin", pedido.AdminId);
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", pedido.ArregloId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "CorreoCliente", pedido.ClienteId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OcasionPedido,ClienteId,NombreCliente,DireccionEnvio,MensajePedido,ModoPago,MontoTotal,EstadoPedido,Servicio,FechaHoraEntrega,DireccionEvento,TematicaEvento,ColoresEvento,AdminId,ArregloId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Admin, "Id", "CorreoAdmin", pedido.AdminId);
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", pedido.ArregloId);
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "CorreoCliente", pedido.ClienteId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Admin)
                .Include(p => p.Arreglo)
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
