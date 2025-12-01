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
    public class ArregloInventariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArregloInventariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ArregloInventarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ArregloInventario.Include(a => a.Arreglo).Include(a => a.Inventario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ArregloInventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arregloInventario = await _context.ArregloInventario
                .Include(a => a.Arreglo)
                .Include(a => a.Inventario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arregloInventario == null)
            {
                return NotFound();
            }

            return View(arregloInventario);
        }

        // GET: ArregloInventarios/Create
        public IActionResult Create()
        {
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo");
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto");
            return View();
        }

        // POST: ArregloInventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArregloId,InventarioId,CantidadNecesaria")] ArregloInventario arregloInventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arregloInventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", arregloInventario.ArregloId);
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", arregloInventario.InventarioId);
            return View(arregloInventario);
        }

        // GET: ArregloInventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arregloInventario = await _context.ArregloInventario.FindAsync(id);
            if (arregloInventario == null)
            {
                return NotFound();
            }
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", arregloInventario.ArregloId);
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", arregloInventario.InventarioId);
            return View(arregloInventario);
        }

        // POST: ArregloInventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArregloId,InventarioId,CantidadNecesaria")] ArregloInventario arregloInventario)
        {
            if (id != arregloInventario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arregloInventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArregloInventarioExists(arregloInventario.Id))
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
            ViewData["ArregloId"] = new SelectList(_context.Arreglo, "Id", "NombreArreglo", arregloInventario.ArregloId);
            ViewData["InventarioId"] = new SelectList(_context.Inventario, "Id", "NombreProducto", arregloInventario.InventarioId);
            return View(arregloInventario);
        }

        // GET: ArregloInventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arregloInventario = await _context.ArregloInventario
                .Include(a => a.Arreglo)
                .Include(a => a.Inventario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arregloInventario == null)
            {
                return NotFound();
            }

            return View(arregloInventario);
        }

        // POST: ArregloInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arregloInventario = await _context.ArregloInventario.FindAsync(id);
            if (arregloInventario != null)
            {
                _context.ArregloInventario.Remove(arregloInventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArregloInventarioExists(int id)
        {
            return _context.ArregloInventario.Any(e => e.Id == id);
        }
    }
}
