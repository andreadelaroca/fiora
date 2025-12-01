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
    public class ArreglosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArreglosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Arreglos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Arreglo.ToListAsync());
        }

        // GET: Arreglos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arreglo = await _context.Arreglo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arreglo == null)
            {
                return NotFound();
            }

            return View(arreglo);
        }

        // GET: Arreglos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Arreglos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TemporadaArreglo,OcasionArreglo,NombreArreglo,TipoArreglo,PrecioArreglo,TiempoEstimadoHoras")] Arreglo arreglo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arreglo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(arreglo);
        }

        // GET: Arreglos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arreglo = await _context.Arreglo.FindAsync(id);
            if (arreglo == null)
            {
                return NotFound();
            }
            return View(arreglo);
        }

        // POST: Arreglos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TemporadaArreglo,OcasionArreglo,NombreArreglo,TipoArreglo,PrecioArreglo,TiempoEstimadoHoras")] Arreglo arreglo)
        {
            if (id != arreglo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arreglo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArregloExists(arreglo.Id))
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
            return View(arreglo);
        }

        // GET: Arreglos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arreglo = await _context.Arreglo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arreglo == null)
            {
                return NotFound();
            }

            return View(arreglo);
        }

        // POST: Arreglos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arreglo = await _context.Arreglo.FindAsync(id);
            if (arreglo != null)
            {
                _context.Arreglo.Remove(arreglo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArregloExists(int id)
        {
            return _context.Arreglo.Any(e => e.Id == id);
        }
    }
}
