using Microsoft.AspNetCore.Mvc;
using Reserva_Espectaculo.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Espectaculo.ViewModels
{
    public class RegistroUsuario
    {
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = ErrorMessage.NotValid)]
        [Remote(action: "ExisteEmail", controller: "Account")]
        public string Email { get; set;}

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.ConfirmPassword)]
        [Compare("Password", ErrorMessage = ErrorMessage.PassMissMatch)]
        public string ConfirmacionPassword { get; set; }
    }
}
