    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    public class DireccionesController : Controller
    {
        private readonly ReservaEspectaculosContext _context;

        public DireccionesController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

        // GET: Direccions
        public IActionResult Index()
        {
            var reservaEspectaculosContext = _context.Direcciones.Include(d => d.Persona);
            return View( reservaEspectaculosContext.ToList());
        }

        private IEnumerable<Persona> getPersonaSinDireccion()
        {
            return _context.Set<Persona>().Include(p => p.Direccion).Where(p => p.Direccion == null);
        }

        // GET: Direccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direccions/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(getPersonaSinDireccion(), "Id", "Apellido");
            return View();
        }

        // POST: Direccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Calle,Altura,Ciudad,CodigoPostal")] Direccion direccion)
        {
            //if (DireccionExists(direccion.Id))
            //{
            //    ModelState.AddModelError("Id", "Esta persona ya tiene una direccion asociada");
            //}
            string errorNoEsperado = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(direccion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e)
            {
                SqlException ie = e.InnerException as SqlException; 

                if (ie != null && (ie.Number == 2627 || ie.Number == 2601))
                {
                    ModelState.AddModelError("Id", "Esta persona ya tiene una direccion asociada");
                }
                else
                {
                    errorNoEsperado = $"Error no esperado al actualizar la DB: {e.InnerException.Message}";
                }
            }
            catch (Exception e)
            {
                errorNoEsperado = $"Error no esperado:: {e.InnerException.Message}";
            }

            if (!string.IsNullOrEmpty(errorNoEsperado))
            {
                ModelState.AddModelError(string.Empty, errorNoEsperado);
            }  
            
            ViewData["Id"] = new SelectList(getPersonaSinDireccion(), "Id", "Apellido", direccion.Id);
            return View(direccion);
        }

        // GET: Direccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(getPersonaSinDireccion(), "Id", "Apellido", direccion.Id);
            return View(direccion);
        }

        // POST: Direccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Calle,Altura,Ciudad,CodigoPostal")] Direccion direccion)
        {
            if (id != direccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id))
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
            ViewData["Id"] = new SelectList(getPersonaSinDireccion(), "Id", "Apellido", direccion.Id);
            return View(direccion);
        }

        // GET: Direccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            _context.Direcciones.Remove(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.Id == id);
        }
    }
}
