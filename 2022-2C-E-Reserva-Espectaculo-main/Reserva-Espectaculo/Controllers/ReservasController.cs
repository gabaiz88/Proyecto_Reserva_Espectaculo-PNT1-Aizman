using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaEspectaculosContext _context;
        private readonly UserManager<Persona> _userManager;
        public ReservasController(ReservaEspectaculosContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Reservas
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Index()
        {
            var reservaEspectaculosContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion);
            return View(await reservaEspectaculosContext.ToListAsync());
        }

        [Authorize(Roles = "Empleado,Admin")]
        public IActionResult Recaudacion(int? idPelicula, Mes mes)
        {
            if(idPelicula == null)
            {
                idPelicula = _context.Peliculas.FirstOrDefault().Id;
            }
            

            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");

            List<Reserva> reservaEspectaculos = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .Include(r => r.Funcion.Pelicula)
                .Include(r => r.Funcion.Sala.TipoSala)
                .Where(r => r.Funcion.PeliculaId == idPelicula 
                && r.Funcion.FechaYHora.Month == ((int) mes) + 1
                && r.Funcion.FechaYHora.Year == DateTime.Now.Year ).ToList();

            float sum = 0;
            reservaEspectaculos.ForEach(r =>
            {
                sum += r.Funcion.Sala.TipoSala.Precio * r.CantidadButacas;
            });
            ViewData["Reservas"] = reservaEspectaculos;
            ViewBag.Total = sum;
            return View();
        }

        // GET: Mis Reservas
        public IActionResult MisReservas()
        {
            var user = User.Identity.Name;
            List<Reserva> reservaEspectaculosFuturas = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .Include(r => r.Funcion.Pelicula)
                .Include(r => r.Funcion.Sala.TipoSala)
                .Where(r => r.Cliente.Email == user && r.Funcion.FechaYHora >= DateTime.Now).ToList();

            List<Reserva> reservaEspectaculosPasadas = _context.Reservas
                .Include(r => r.Cliente).Include(r => r.Funcion)
                .Include(r => r.Funcion.Pelicula)
                .Include(r => r.Funcion.Sala.TipoSala)
                .Where(r => r.Cliente.Email == user && r.Funcion.FechaYHora < DateTime.Now).ToList();

            ViewBag.ReservasFuturas = reservaEspectaculosFuturas;

            ViewBag.ReservasPasadas = reservaEspectaculosPasadas;

            return View();
        }

        // GET: Reservas/Details/5
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        [HttpGet]
        public IActionResult Create(int? idPelicula)
        {
            Pelicula pelicula = _context.Peliculas.Find(idPelicula);
            ViewBag.Pelicula = pelicula;
            ViewData["Funciones"] = new SelectList(_context.Funciones
                .Include(f => f.Pelicula)
                .Where(f => f.Pelicula.Id == idPelicula 
                && f.ButacasDisponibles > 0 
                && f.Confirmada == true
                && (f.FechaYHora >= DateTime.Now && f.FechaYHora <= DateTime.Now.AddDays(7)))
                .ToList(),"Id","FechaYHora");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadButacas,FuncionId")] Reserva reserva)
        {
            Funcion funcion = _context.Funciones.Include(f => f.Pelicula).FirstOrDefault(f => f.Id == reserva.FuncionId);
            Pelicula pelicula = funcion.Pelicula;

            if (ModelState.IsValid)
            {
                var user = User.Identity.Name;
                reserva.ClienteId = _context.Clientes.FirstOrDefault(c => c.Email == user).Id;

                if (!ClienteTieneReserva(user, reserva.FuncionId))
                {

                    if (HayLugar(reserva.FuncionId, reserva.CantidadButacas))
                    {
                        ReservarAsiento(reserva.FuncionId, reserva.CantidadButacas);
                        CrearRelacionFuncionXCliente(reserva.FuncionId, reserva.ClienteId);
                        _context.Add(reserva);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(MisReservas));
                    }
                    else
                    {
                        //pelicula = _context.Peliculas.Find(id);
                        ViewBag.Pelicula = pelicula;
                        ViewData["Funciones"] = new SelectList(_context.Funciones.Include(f => f.Pelicula).Where(f => f.Pelicula.Id == pelicula.Id && f.ButacasDisponibles > 0 && f.Confirmada == true).ToList(), "Id", "FechaYHora");
                        ModelState.AddModelError(String.Empty, "La cantidad de asientos indicada no se encuentra disponible");
                        
                    }

                }
                else
                {
                    //pelicula = _context.Peliculas.Find(id);
                    ViewBag.Pelicula = pelicula;
                    ViewData["Funciones"] = new SelectList(_context.Funciones.Include(f => f.Pelicula).Where(f => f.Pelicula.Id == pelicula.Id && f.ButacasDisponibles > 0 && f.Confirmada == true).ToList(), "Id", "FechaYHora");
                    ModelState.AddModelError(String.Empty,"El cliente ya tiene una reserva.");
                }
            }

            
            
            //return RedirectToAction("Create?idPelicula="+pelicula.Id, reserva);
            //TempData["RedirectModel"] = reserva;
            return View(reserva);
        }

        private bool ClienteTieneReserva(string email, int idFuncion)
        {
            Cliente cliente = _context.Clientes.FirstOrDefault(c => c.Email == email);
            return _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .Any(r => r.Cliente.Id == cliente.Id && r.Funcion.Id == idFuncion);
        }

        // GET: Reservas/Edit/5
        [Authorize(Roles = "Empleado,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [Authorize(Roles = "Empleado,Admin")]
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaAlta,CantidadButacas,ClienteId,FuncionId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reserva = _context.Reservas.Include(r => r.Funcion).FirstOrDefault(r => r.Id == id);
            if (reserva.Funcion.FechaYHora.AddHours(-24) < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Las reservas solo pueden cancelarse con 24hs de anticipación";
                return RedirectToAction("MisReservas");
            }
            if(reserva != null)
            {
                DesasignarAsiento(reserva.FuncionId, reserva.CantidadButacas);
                EliminarRelacionFuncionXCliente(reserva.FuncionId, reserva.ClienteId);
                _context.Reservas.Remove(reserva);
                 _context.SaveChanges();
            }
            
            

            if (User.IsInRole("Admin") || User.IsInRole("Empleado")) 
            { 
            return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(MisReservas));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

        //Listar al usuario funciones disponibles por película
        // (Se puede agregar filtro para mostrar solo las que tienen asientos disponibles)
        public List<Funcion> FuncionesPorPelicula(int idPelicula)
        {
            return _context.Funciones.Include(f => f.Sala).Include(f => f.Pelicula).Where(f => f.Pelicula.Id == idPelicula).ToList();
        }

        //Al momento de hacer la reserva verificar que haya lugar en la sala para esa función
        private bool HayLugar(int id, int cantidadAsientos)
        {
            var funcion = _context.Funciones.Find(id);
            return funcion.ButacasDisponibles >= cantidadAsientos;
        }

        //Al momento de crear la reserva descontar el lugar diponible para la función
        private void ReservarAsiento(int id, int cantidadAsientos)
        {
            if (HayLugar(id, cantidadAsientos))
            {
                var funcion = _context.Funciones.Find(id);
                funcion.ButacasDisponibles -= cantidadAsientos;
                _context.Update(funcion);
                _context.SaveChanges();

            }
            
        }

        //Al momento de cancelar la reserva diponibilidar esos asientos para la función
        private void DesasignarAsiento(int id, int cantidadAsientos)
        {

            var funcion = _context.Funciones.Find(id);
            funcion.ButacasDisponibles += cantidadAsientos;
            _context.Update(funcion);
            _context.SaveChanges();

        }

        public void CrearRelacionFuncionXCliente(int idFuncion, int idCliente)
        {
            bool existeRelacion = _context.FuncionCliente.Any(fc => fc.FuncionId == idFuncion && fc.ClienteId == idCliente);
            if (!existeRelacion)
            {
                FuncionCliente relacion = new FuncionCliente(idFuncion, idCliente);
                _context.Add(relacion);
                _context.SaveChanges();
            }

        }

        public void EliminarRelacionFuncionXCliente(int idFuncion, int idCliente)
        {
            FuncionCliente relacion = _context.FuncionCliente.FirstOrDefault(fc => fc.FuncionId == idFuncion && fc.ClienteId == idCliente);
            if (relacion != null)
            {
                _context.Remove(relacion);
                _context.SaveChanges();
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PrecioSalaPorFuncion(int? idFuncion)
        {
            float price = (float) _context.Funciones.Include(f => f.Sala).Include(f => f.Sala.TipoSala).FirstOrDefault(f => f.Id == idFuncion).Sala.TipoSala.Precio;
            //var price = "Price";
            return Json(new { precio = price});

        }

        public IActionResult ReservasPorFuncion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Funcion funcion = _context.Funciones
                .Include(f => f.Reservas)
                .ThenInclude(r => r.Cliente)
                .FirstOrDefault(m => m.Id == id);

            List<Reserva> reservas = funcion.Reservas;

            if (funcion == null)
            {
                return NotFound();
            }

            return View("Index", reservas);
        }
    }
}
