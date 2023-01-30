using Microsoft.VisualBasic;
using Reserva_Espectaculo.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Espectaculo.Models
{
    public class Reserva
    {
        public int Id { get; protected set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [Range(1, Int32.MaxValue, ErrorMessage = ErrorMessage.PositiveNumber)]
        public int CantidadButacas { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        public Reserva()
        {

        }
        public Reserva(int Id, Funcion Funcion, Cliente Cliente, int CantidadButacas, int ClienteId, int FuncionId)
        {
            this.Id = Id;
            this.Funcion = Funcion;
            this.FuncionId = FuncionId;
            this.FechaAlta = DateTime.Today;
            this.Cliente = Cliente;
            this.ClienteId = ClienteId;
            this.CantidadButacas = CantidadButacas;
        }
    }
}
