using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Espectaculo.Models
{
    public class Cliente : Persona
    {
        public List<Reserva> Reservas { get; set; }
        public List<FuncionCliente> ClienteFunciones { get; set; }

        public Cliente()
        {
        }

        public Cliente(string nombre, string apellido, int dni, string email, int telefono, string calle, int altura, string ciudad)
          : base(nombre, apellido, dni, email, telefono, calle, altura, ciudad)
        {

        }
    }
}
