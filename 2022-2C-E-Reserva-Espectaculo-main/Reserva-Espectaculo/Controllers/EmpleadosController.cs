using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_Espectaculo.Helpers;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ReservaEspectaculosContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        public EmpleadosController(ReservaEspectaculosContext context, UserManager<Persona> userManager, SignInManager<Persona> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Empleados
        public IActionResult Index()
        {
            return View( _context.Empleados.ToList());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        public async Task<IActionResult> DetailsByUsername()
        {
            var username = User.Identity.Name;
            if (username == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.UserName == username);
            if (empleado == null)
            {
                return NotFound();
            }

            return View("Details",empleado);
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Legajo,Nombre,Apellido,Dni,Email,Telefono,FechaAlta")] Empleado empleado)
        {
                if (ModelState.IsValid)
                {
                    empleado.UserName = empleado.Email;
                    empleado.NormalizedUserName = empleado.Email.ToUpper();
                    empleado.NormalizedEmail = empleado.Email.ToUpper();
                    var resultadoCreate = await _userManager.CreateAsync(empleado, Configs.PasswordAdmin);

                if (resultadoCreate.Succeeded)
                {
                    IdentityResult resultadoAddRole = await _userManager.AddToRoleAsync(empleado, Configs.EmpleadoRoleName);

                    if (!resultadoAddRole.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, $"No se pudo agregar el rol de {Configs.EmpleadoRoleName}");
                    }
                }

                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Legajo,Nombre,Apellido,Dni,Email,Telefono,FechaAlta")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
