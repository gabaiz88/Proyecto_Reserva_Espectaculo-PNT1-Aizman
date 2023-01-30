using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_Espectaculo.Helpers;
using Reserva_Espectaculo.Models;

namespace Reserva_Espectaculo.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ReservaEspectaculosContext _context;
        private readonly UserManager<Persona> _userManager;

        public PersonasController(ReservaEspectaculosContext context,UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Personas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persona.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        [Authorize(Roles = "Empleado,Admin")]
        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool esAdmin,[Bind("Id,Nombre,Apellido,Dni,Email,Telefono,FechaAlta")] Persona persona)
        {
            if (ModelState.IsValid)
            {


                //_context.Add(persona);
                //await _context.SaveChangesAsync();

                persona.UserName = persona.Email;
                var resultadoNewPersona = await _userManager.CreateAsync(persona,Configs.PasswordAdmin);
                if (resultadoNewPersona.Succeeded)
                {
                    IdentityResult resultadoAddRole;
                    string rolDefinido;

                    if (esAdmin)
                    {
                        rolDefinido = Configs.AdminRoleName;
                    }
                    else
                    {
                        rolDefinido = Configs.ClienteRoleName;
                    }

                    resultadoAddRole = await _userManager.AddToRoleAsync(persona, rolDefinido);

                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction("Index", "Personas");
                    }
                    else
                    {
                        return Content($"No se ha podido agregar el rol {rolDefinido}");
                    }
                }

                foreach(var error in resultadoNewPersona.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Email,Telefono,FechaAlta")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Persona.FindAsync(id);
            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.Id == id);
        }
    }
}
