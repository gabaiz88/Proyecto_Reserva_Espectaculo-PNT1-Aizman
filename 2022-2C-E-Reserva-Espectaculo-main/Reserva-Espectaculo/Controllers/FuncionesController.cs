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
    public class FuncionesController : Controller
    {
        private readonly ReservaEspectaculosContext _context;
        //private readonly SalasController _salasController;

        public FuncionesController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

        
        // GET: Funciones
        public IActionResult Index()
        {
            List<Funcion> funcionesFuturas = _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).Where(f => f.FechaYHora >= DateTime.Now).ToList();
            List<Funcion> funcionesPasadas = _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).Where(f => f.FechaYHora < DateTime.Now).ToList();

            ViewBag.FuncionesFuturas = funcionesFuturas;

            ViewBag.FuncionesPasadas = funcionesPasadas;

            return View();
        }

        // GET: Funciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

      
        // GET: Funciones/Create
        [Authorize(Roles = "Empleado,Admin")]
        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Set<Pelicula>(), "Id", "Titulo");
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero");
            return View();
        }

        // POST: Funciones/Create
        [Authorize(Roles = "Empleado,Admin")]
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaYHora,Descripcion,Confirmada,Duracion,PeliculaId,SalaId")] Funcion funcion)
        {
            VerificarPeliculaEnFechaYHora(funcion);
            if (ModelState.IsValid)
            {
                if (ExisteFuncionReservadaEnEsaSala(funcion.SalaId,funcion.FechaYHora))
                {
                    ModelState.AddModelError(String.Empty, "La sala no se encuentra disponible en el día y horario solicitado");
                    return View(funcion);
                }
                funcion.ButacasDisponibles = _context.Salas.Find(funcion.SalaId).CapacidadButacas;
                _context.Add(funcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            ViewData["PeliculaId"] = new SelectList(_context.Set<Pelicula>(), "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
            return View(funcion);
        }

        private void VerificarPeliculaEnFechaYHora(Funcion funcion)
        {
           
            if (ExisteFuncionReservadaDePeliculaEnFechaYHora(funcion))
            {
                ModelState.AddModelError("Pelicula", "Ya se encuentra una funcion de esa Pelicula en esta fecha y hora");
            }
        }
        // GET: Funciones/Edit/5
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }
            ViewData["PeliculaId"] = new SelectList(_context.Set<Pelicula>(), "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
            return View(funcion);
        }

        // POST: Funciones/Edit/5
        [Authorize(Roles = "Empleado,Admin")]
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaYHora,Descripcion,ButacasDisponibles,Confirmada,Duracion,PeliculaId,SalaId")] Funcion funcion)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }
            VerificarPeliculaEnFechaYHora(funcion);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
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
            ViewData["PeliculaId"] = new SelectList(_context.Set<Pelicula>(), "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero", funcion.SalaId);
            return View(funcion);
        }

        // GET: Funciones/Delete/5
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // POST: Funciones/Delete/5
        [Authorize(Roles = "Empleado,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Funcion funcion = await _context.Funciones
                .Include(f => f.Reservas).FirstOrDefaultAsync(f => f.Id == id);
            if (funcion.Reservas.Count() > 0)
            {
                TempData["ErrorMessage"] = "No puede eliminarse la función porque posee rreservas acociadas";
                return RedirectToAction("Index");
            }
            _context.Funciones.Remove(funcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Funcion BuscarPorPeliculaYFechaHora(int idPelicula, DateTime fechaYHora)
        {
            return _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefault(m => m.Pelicula.Id == idPelicula && m.FechaYHora == fechaYHora);
        }

        //Ver disponibilidad de una sala en fecha y horario
        public bool ExisteFuncionReservadaEnEsaSala(int idSala, DateTime fechaYHora)
        {
            return _context.Funciones.Include(f => f.Sala).Any(f => (f.Sala.Id == idSala) &&
            (fechaYHora >= f.FechaYHora && fechaYHora < f.FechaYHora.AddHours(f.Duracion)));
             
        }

        //public bool ExisteFuncionReservadaDePeliculaEnFechaYHora(Pelicula pelicula, DateTime fechaYHora)
        //{
        //    return _context.Funciones.Include(f => f.Pelicula).Any(f => (f.Pelicula.Titulo == pelicula.Titulo) &&
         //   (fechaYHora >= f.FechaYHora && fechaYHora < f.FechaYHora.AddHours(f.Duracion)));

        //}

        public bool ExisteFuncionReservadaDePeliculaEnFechaYHora(Funcion funcion)
        {
            bool resultado = false;
            if (funcion.Pelicula != null && funcion.FechaYHora != null)
            {
                if (funcion.Id != null && funcion.Id != 0)
                {
                    resultado = _context.Funciones.Include(f => f.Pelicula).Any(f => (f.Pelicula == funcion.Pelicula) &&
            (funcion.FechaYHora >= f.FechaYHora && funcion.FechaYHora < f.FechaYHora.AddHours(f.Duracion)) && f.Id != funcion.Id);
                }
                else
                {
                    resultado = _context.Funciones.Include(f => f.Pelicula).Any(f => (f.Pelicula == funcion.Pelicula) &&
            (funcion.FechaYHora >= f.FechaYHora && funcion.FechaYHora < f.FechaYHora.AddHours(f.Duracion)));
                }
            }
            return resultado;
        }
        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.Id == id);
        }

    }
}
