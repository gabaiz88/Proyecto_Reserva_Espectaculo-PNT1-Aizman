using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    [Authorize(Roles = "Empleado,Admin")]
    public class SalasController : Controller
    {
        private readonly ReservaEspectaculosContext _context;

        //private readonly FuncionesController _funcionesController;

        public SalasController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Salas
        public IActionResult Index()
        {
            var reservaEspectaculosContext = _context.Salas.Include(s => s.TipoSala);
            return View(reservaEspectaculosContext.ToList());
        }

        // GET: Salas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Salas/Create
        public IActionResult Create()
        {
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre");
            return View();
        }

        // POST: Salas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,CapacidadButacas,TipoSalaId")] Sala sala)
        {
            VerificarNum(sala);
            if (ModelState.IsValid)
            {
                _context.Add(sala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        private void VerificarNum(Sala sala)
        {
            if (ExistNum(sala))
            {
                ModelState.AddModelError("Numero", "El numero de Sala ya esta creado");
            }
        }
        public bool ExistNum(Sala sala)
        {
            bool resultado = false;
            if (sala.Numero > 0)
            {
                if (sala.Id != null && sala.Id != 0)
                {
                    resultado = _context.Salas.Any(s => s.Numero == sala.Numero && s.Id != sala.Id);
                }
                else
                {
                    resultado = _context.Salas.Any(s => s.Numero == sala.Numero);
                }
            }
            return resultado;
        }
        // GET: Salas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,CapacidadButacas,TipoSalaId")] Sala sala)
        {
            
            if (id != sala.Id)
            {
                return NotFound();
            }

            VerificarNum(sala);
            if (ModelState.IsValid)
            {
                try
                {
                   
                        _context.Update(sala);
                        await _context.SaveChangesAsync();
                    
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaExists(sala.Id))
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
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        // GET: Salas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }


        //Al momento de crear la función buscar salas disponibles (Que no estén reservadas para otra función en la misma fecha y horario)
        /*public List<Sala> ListarSalasDisponiblesPorFechaYHora(DateTime fechaYHora)
        {
            List<Sala> salas = _context.Salas.ToList();
            return salas.Where(s => _funcionesController.ExisteFuncionReservadaEnEsaSala(s.Id, fechaYHora)).ToList();
        }*/
    }
}
