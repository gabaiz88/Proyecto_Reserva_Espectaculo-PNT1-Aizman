using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Sala
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public int Numero { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [Range(1, Int32.MaxValue, ErrorMessage = ErrorMessage.PositiveNumber)]
        public int CapacidadButacas { get; set; }

        public int TipoSalaId { get; set; }
        public TipoSala TipoSala { get; set; }
        
        public List<Funcion>? Funciones { get; set; }

        public Sala()
        {

        }
        public Sala(int numero, int capacidadButacas, int tipoSalaId)
        {
            this.Numero = numero;
            this.CapacidadButacas = capacidadButacas;
            this.TipoSalaId = tipoSalaId;
        }
    }
}
