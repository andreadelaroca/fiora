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
    public class PedidoInventariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoInventariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PedidoInventarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PedidoInventario.Include(p => p.Inventario).Include(p => p.Pedido);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PedidoInventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoInventario = await _context.PedidoInventario
                .Include(p => p.Inventario)
                .Include(p => p.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoInventario == null)
            {
                return NotFound();
            }

            return View(pedidoInventario);
        }

        // GET: PedidoInventarios/Create
        public IActionResult Create()
        {
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto");
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "DireccionEnvio");
            return View();
        }

        // POST: PedidoInventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PedidoId,InventarioId,Cantidad,PrecioUnitario")] PedidoInventario pedidoInventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoInventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", pedidoInventario.InventarioId);
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "DireccionEnvio", pedidoInventario.PedidoId);
            return View(pedidoInventario);
        }

        // GET: PedidoInventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoInventario = await _context.PedidoInventario.FindAsync(id);
            if (pedidoInventario == null)
            {
                return NotFound();
            }
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", pedidoInventario.InventarioId);
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "DireccionEnvio", pedidoInventario.PedidoId);
            return View(pedidoInventario);
        }

        // POST: PedidoInventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PedidoId,InventarioId,Cantidad,PrecioUnitario")] PedidoInventario pedidoInventario)
        {
            if (id != pedidoInventario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoInventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoInventarioExists(pedidoInventario.Id))
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
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", pedidoInventario.InventarioId);
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "DireccionEnvio", pedidoInventario.PedidoId);
            return View(pedidoInventario);
        }

        // GET: PedidoInventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoInventario = await _context.PedidoInventario
                .Include(p => p.Inventario)
                .Include(p => p.Pedido)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoInventario == null)
            {
                return NotFound();
            }

            return View(pedidoInventario);
        }

        // POST: PedidoInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoInventario = await _context.PedidoInventario.FindAsync(id);
            if (pedidoInventario != null)
            {
                _context.PedidoInventario.Remove(pedidoInventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoInventarioExists(int id)
        {
            return _context.PedidoInventario.Any(e => e.Id == id);
        }
    }
}
