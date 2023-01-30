using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reserva_Espectaculo.Helpers;
using Reserva_Espectaculo.Models;
using Reserva_Espectaculo.ViewModels;
using System;
using System.Threading.Tasks;

namespace Reserva_Espectaculo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signInManager)
        {
            this._usermanager = usermanager;
            this._signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registrar()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("Email,Password,ConfirmacionPassword")] RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                Cliente clienteACrear = new Cliente()
                {
                    Email = viewModel.Email,
                    UserName = viewModel.Email
                };

                var resultadoCreate = await _usermanager.CreateAsync(clienteACrear, Configs.PasswordAdmin);
                
                if (resultadoCreate.Succeeded)
                {

                    IdentityResult resultadoAddRole = await _usermanager.AddToRoleAsync(clienteACrear, Configs.ClienteRoleName);

                    if (resultadoAddRole.Succeeded)
                    {
                        await _signInManager.SignInAsync(clienteACrear, isPersistent: false);

                        return RedirectToAction("Edit", "Clientes", new { id = clienteACrear.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"No se pudo agregar el rol de {Configs.ClienteRoleName}");
                    }

                }

                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }
            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme,false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "Inicio de sesion inválido");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");   
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExisteEmail(string email)
        {
            var exiteEmailRegistrado = await _usermanager.FindByEmailAsync(email);

            if(exiteEmailRegistrado == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"El email { email } ya se encuentra registrado");
            }
        }

    }
}

