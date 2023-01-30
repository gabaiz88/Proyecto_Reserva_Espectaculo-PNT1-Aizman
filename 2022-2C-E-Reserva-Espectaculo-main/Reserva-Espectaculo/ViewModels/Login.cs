using Reserva_Espectaculo.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Reserva_Espectaculo.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = ErrorMessage.NotValid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        public bool Recordarme { get; set; }
    }
}
