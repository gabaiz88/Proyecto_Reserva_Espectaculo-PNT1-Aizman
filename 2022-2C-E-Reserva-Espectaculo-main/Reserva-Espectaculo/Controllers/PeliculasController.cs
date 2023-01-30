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
    public class PeliculasController : Controller
    {
        private readonly ReservaEspectaculosContext _context;

        public PeliculasController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

       
        // GET: Peliculas
        public IActionResult Index(TipoDeGenero? genero)
        {
            List<Pelicula> peliculas;
            if(genero == null)
            {
                genero = TipoDeGenero.ACCION;
            }
            
            peliculas = _context.Peliculas
                    .Where(p => p.Genero.Equals(genero)).ToList();

            TempData["SelectedGender"] = genero != null? genero: TipoDeGenero.ACCION;
            return View(peliculas);
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Titulo,FechaLanzamiento,Descripcion,GeneroId,Genero")] Pelicula pelicula)
        {
            VerificarTitulo(pelicula);
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,FechaLanzamiento,Descripcion,GeneroId,Genero")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            VerificarTitulo(pelicula);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        public bool ExistTitulo(Pelicula pelicula)
        {
            bool resultado = false;
            if (!string.IsNullOrEmpty(pelicula.Titulo))
            {
                if (pelicula.Id != null && pelicula.Id != 0)
                {
                    resultado = _context.Peliculas.Any(p => p.Titulo == pelicula.Titulo && p.Id != pelicula.Id);
                }
                else
                {
                    resultado = _context.Peliculas.Any(p => p.Titulo == pelicula.Titulo);
                }
            }
            return resultado;
        }

        private void VerificarTitulo(Pelicula pelicula)
        {
            if (ExistTitulo(pelicula))
            {
                ModelState.AddModelError("Titulo", "Ya existe una pelicula con ese titulo.");
            }
        }
        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }

        public ActionResult PeliculasPorGenero(TipoDeGenero genero)
        {
            return View(_context.Peliculas.Where(p => p.Genero == genero).ToList());
        }
    }
}
