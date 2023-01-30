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
    public class ClientesController : Controller
    {
        private readonly ReservaEspectaculosContext _context;

        public ClientesController(ReservaEspectaculosContext context)
        {
            _context = context;
        }

        // GET: Clientes
        [Authorize(Roles = "Empleado,Admin")]
        public IActionResult Index()
        {
            return View(_context.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public async Task<IActionResult> DetailsByUsername()
        {
            var username = User.Identity.Name;
            if (username == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.UserName == username);
            if (cliente == null)
            {
                return NotFound();
            }

            return View("Details", cliente);
        }

        // GET: Clientes/Create
        [Authorize(Roles = "Empleado,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Dni,Email,Telefono,Direccion,FechaAlta,UsuarioId")] Cliente cliente)
        {


            VerificarEmail(cliente);
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public IActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Email,Telefono,Direccion,FechaAlta")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            VerificarEmail(cliente);

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDb = _context.Clientes.Find(cliente.Id);
                    if(clienteEnDb == null)
                    {
                        return NotFound();
                    }
                    clienteEnDb.Nombre = cliente.Nombre;
                    clienteEnDb.Apellido = cliente.Apellido;
                    clienteEnDb.Dni = cliente.Dni;
                    clienteEnDb.Telefono = cliente.Telefono;
                    clienteEnDb.FechaAlta = cliente.FechaAlta;
                    clienteEnDb.Direccion = cliente.Direccion;  

                    if (!ActualizarEmail(cliente, clienteEnDb))
                    {
                        ModelState.AddModelError("Email", "El email ya esta en uso");
                        return View(cliente);
                    }

                    _context.Update(clienteEnDb);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Details", cliente);
            }
            return View(cliente);
        }

      
        public bool ActualizarEmail(Cliente clienteForm,Cliente clienteEnDb)
        {
            bool res = true;
            try
            {
                if (!clienteEnDb.NormalizedEmail.Equals(clienteForm.Email.ToUpper()))
                {
                    if (ExistEmail(clienteForm))
                    {
                        res = false;
                    }
                    else
                    {
                        clienteEnDb.Email = clienteForm.Email;
                        clienteEnDb.NormalizedEmail = clienteForm.Email.ToUpper();
                        clienteEnDb.UserName = clienteForm.Email;
                        clienteEnDb.NormalizedUserName = clienteForm.NormalizedEmail;
                    }
                }
                else
                {

                }
            }
            catch
            {
                res = false;
            }
            return res;
        }

        public bool ExistEmail(Cliente cliente)
        {
           bool resultado = false;
            if (!string.IsNullOrEmpty(cliente.Email))
            {
                if(cliente.Id != null && cliente.Id != 0)
                {
                    resultado = _context.Clientes.Any(c => c.Email == cliente.Email && c.Id != cliente.Id);
                }
                else
                {
                    resultado = _context.Clientes.Any(c => c.Email == cliente.Email);
                }
            }
            return resultado;
        }

        private void VerificarEmail(Cliente cliente)
        {
            if (ExistEmail(cliente))
            {
                ModelState.AddModelError("Email", "El email ya se encuentra registrado");
            }
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
