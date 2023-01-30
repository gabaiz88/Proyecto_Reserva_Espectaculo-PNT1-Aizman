using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reserva_Espectaculo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reserva_Espectaculo.Helpers;
using System;

namespace Reserva_Espectaculo.Controllers
{
    public class PrecargaDB : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ReservaEspectaculosContext _context;

        public PrecargaDB(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ReservaEspectaculosContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;

        }

        private List<string> roles = new List<string>() { "Admin", "Cliente", "Empleado" };
        private List<Empleado> empleados = new List<Empleado>()
        {
            new Empleado("Karina", "Auday", 40123123, "kauday@gmail.com", 123456, "salguero", 600, "buenos aires", "AdminP-12345678-ad"),
            new Empleado("Gabriel", "Aizman", 41034851, "jaizman@gmail.com", 1134998855, "Yatay", 500, "caba", "EmpleadoF-12345678-ad"),
            new Empleado("Juan", "Pulleiro", 41034852, "jpulleiro@gmail.com", 1134998855, "Yatay", 500, "caba", "EmpleadoF-12345678-ad"),
            new Empleado("Pedro", "Gutiérrez", 41034853, "jgutierrez@gmail.com", 1134998855, "Yatay", 500, "caba", "EmpleadoF-12345678-ad"),
            new Empleado("Florencia", "Zabala", 41034854, "fzabala@gmail.com", 1134998855, "Yatay", 500, "caba", "EmpleadoF-12345678-ad")
        };

        private List<Cliente> clientes = new List<Cliente>()
        {
            new Cliente("Jose", "Perez", 40123123, "culaquiera@gmail.com", 123456, "salguero", 600, "buenos aires"),
            new Cliente("Pepe", "Gonzalez", 52415613, "culaquiera@gmail.com", 123456, "salguero", 600, "buenos aires"),
            new Cliente("Facundo", "Groso", 23156156, "culaquiera@gmail.com", 123456, "salguero", 600, "buenos aires"),
        };

        //private List<Empleado> empleados = new List<Empleado>() { admin };

        private List<Pelicula> peliculas = new List<Pelicula>() { 
            new Pelicula(new System.DateTime(1984,01,11),"Terminator","El exterminador es una película dirigida por James Cameron con Arnold Schwarzenegger, Linda Hamilton, Michael Biehn, ...", TipoDeGenero.ACCION,"\\img\\peliculas\\terminator.jpg"),
            new Pelicula(new System.DateTime(1985,01,11),"Volver al futuro","Volver al futuro se compone principalmente de un conjunto de tres películas de ciencia ficción estadounidenses dirigidas por Robert Zemeckis.", TipoDeGenero.ACCION, "\\img\\peliculas\\volver_al_futuro.jpg"),
            new Pelicula(new System.DateTime(1968,01,11),"Odisea en el espacio","Odisea en el espacio es una película de culto británico-estadounidense del género ciencia ficción y suspenso dirigida por Stanley Kubrick.", TipoDeGenero.SUSPENSO, "\\img\\peliculas\\odisea_en_el_espacio.jpg"),
            new Pelicula(new System.DateTime(1980,01,11),"El resplandor","El resplandor es una película angloestadounidense de 1980 del subgénero de terror psicológico, producida y dirigida por Stanley Kubrick. ", TipoDeGenero.TERROR,"\\img\\peliculas\\the_shining.jpg")
        };

        private List<TipoSala> tipoSalas = new List<TipoSala>()
        {
            new TipoSala("Sala premium",(float) 246.20),
            new TipoSala("Sala standard",(float) 120.50)
        };

        private List<Sala> salas = new List<Sala>()
        {
            new Sala(1,120,1),
            new Sala(2,200,2),
            new Sala(3,180,1),
            new Sala(4,132,2),
            new Sala(5,120,2)
        };

        private List<Funcion> funciones = new List<Funcion>() {
            new Funcion(new System.DateTime(2023,01,25,10,30,00),"Funcion matutina",true,1,1),
            new Funcion(new System.DateTime(2022,12,20,12,30,00),"Funcion matutina",true,2,1),
            new Funcion(new System.DateTime(2023,01,07,22,30,00),"Funcion matutina",true,3,2),
            new Funcion(new System.DateTime(2022,12,26,18,30,00),"Funcion matutina",true,2,4),
            new Funcion(new System.DateTime(2023,01,02,19,30,00),"Funcion matutina",true,3,2),
            new Funcion(new System.DateTime(2023,01,14,20,30,00),"Funcion matutina",true,1,3),
            new Funcion(new System.DateTime(2022,12,30,21,30,00),"Funcion matutina",true,4,2),
            new Funcion(new System.DateTime(2022,12,18,22,30,00),"Funcion matutina",true,3,1),
        };

       

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Seed()
        {
            CrearRoles().Wait();
            CrearEmpleados().Wait(); 
            CrearPeliculas().Wait();
            CrearTipoSalas().Wait();
            CrearSalas().Wait();
            CrearFunciones().Wait();
            CrearClientes().Wait();
            return RedirectToAction("Index", "Home", (new { mensaje = "Precarga Seed Finalizada" }));
        }

        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                    await _roleManager.CreateAsync(new Rol(rolName));
                }
            }
        }

        private async Task CrearEmpleados()
        {
            foreach (var empleado in empleados)
            {
                if (!_context.Empleados.Any(e => e.Email == empleado.Email))
                {
                    if (empleado.Apellido.Equals("Auday"))
                    {
                        await _userManager.CreateAsync(empleado, Configs.PasswordAdmin);
                        await _userManager.AddToRoleAsync(empleado, "Admin");
                    }
                    else
                    {
                        await _userManager.CreateAsync(empleado, Configs.PasswordAdmin);
                        await _userManager.AddToRoleAsync(empleado, "Empleado");
                    }
                }
                
            }

        }
        private async Task CrearClientes()
        {
            foreach (var cliente in clientes)
            {
                if (!_context.Clientes.Any(e => e.Email == cliente.Email))
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                }
            }

        }


        private async Task CrearPeliculas()
        {
            foreach (var pelicula in peliculas)
            {
                if (! _context.Peliculas.Any(e => e.Titulo == pelicula.Titulo))
                {
                    _context.Add(pelicula);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task CrearTipoSalas()
        {
            foreach (var tipoSala in tipoSalas)
            {
                if (!_context.TipoSalas.Any(e => e.Nombre == tipoSala.Nombre))
                {
                    _context.Add(tipoSala);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task CrearSalas()
        {
            foreach (var sala in salas)
            {
                if (!_context.Salas.Any(e => e.Numero == sala.Numero))
                {
                    sala.TipoSala = _context.TipoSalas.Find(1);
                    _context.Add(sala);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task CrearFunciones()
        {
            foreach (var funcion in funciones)
            {
                if (!_context.Funciones.Any(e => e.PeliculaId == funcion.PeliculaId && e.FechaYHora == funcion.FechaYHora))
                {
                    _context.Add(funcion);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
