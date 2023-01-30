using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Empleado : Persona
    {

        // inical nombre + apellido + id 8 dígitos + siglas area
        //Ejemplo: fzabala-12345678-ad (Administración)
        [RegularExpression(@"^[\D]{1,20}-[\d]{8}-[\D]{2}$",
        ErrorMessage = ErrorMessage.LegajoFormat)]
        public string Legajo { get; set; }

        public Empleado()
        {

        }

        public Empleado(string nombre, string apellido, int dni, string email, int telefono, string calle, int altura, string ciudad, string legajo)
            : base(nombre, apellido, dni, email, telefono, calle, altura, ciudad)
        {
            Legajo = legajo;
        }

    }
}
