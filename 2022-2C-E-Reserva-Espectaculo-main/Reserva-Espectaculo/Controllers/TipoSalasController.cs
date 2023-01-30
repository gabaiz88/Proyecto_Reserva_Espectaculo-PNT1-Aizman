using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    [Authorize(Roles = "Empleado,Admin")]
    public class TipoSalasController : Controller
    {
        private readonly ReservaEspectaculosContext _context;

        public TipoSalasController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

        // GET: TipoSalas
        public IActionResult Index()
        {
            return View(_context.TipoSalas.ToList());
        }

        // GET: TipoSalas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // GET: TipoSalas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSalas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            VerificarNombre(tipoSala);
            if (ModelState.IsValid)
            {
                _context.Add(tipoSala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSala);
        }

        // GET: TipoSalas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas.FindAsync(id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // POST: TipoSalas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            bool hayReservasEnSala = _context.Reservas
                .Include(r => r.Funcion)
                .Include(r => r.Funcion.Sala)
                 .Include(r => r.Funcion.Sala.TipoSala)
                .Any(r => r.Funcion.Sala.TipoSalaId == id && r.Funcion.FechaYHora >= DateTime.Now );

            if (id != tipoSala.Id)
            {
                return NotFound();
            }

            VerificarNombre(tipoSala);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!hayReservasEnSala)
                    {
                        _context.Update(tipoSala);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "El tipo de sala no puede editarse porque existe reservas asociadas");
                        return View();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSalaExists(tipoSala.Id))
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
            return View(tipoSala);
        }

        public bool ExistNombre(TipoSala tipoSala)
        {
            bool resultado = false;
            if (!string.IsNullOrEmpty(tipoSala.Nombre))
            {
                if (tipoSala.Id != null && tipoSala.Id != 0)
                {
                    resultado = _context.TipoSalas.Any(t => t.Nombre == tipoSala.Nombre && t.Id != tipoSala.Id);
                }
                else
                {
                    resultado = _context.TipoSalas.Any(t => t.Nombre == tipoSala.Nombre);
                }
            }
            return resultado;
        }

        private void VerificarNombre(TipoSala tipoSala)
        {
            if (ExistNombre(tipoSala))
            {
                ModelState.AddModelError("Nombre", "Ya existe un tipo de sala con ese Nombre.");
            }
        }
        // GET: TipoSalas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        // POST: TipoSalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSala = await _context.TipoSalas.FindAsync(id);
            _context.TipoSalas.Remove(tipoSala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSalaExists(int id)
        {
            return _context.TipoSalas.Any(e => e.Id == id);
        }
    }
}
