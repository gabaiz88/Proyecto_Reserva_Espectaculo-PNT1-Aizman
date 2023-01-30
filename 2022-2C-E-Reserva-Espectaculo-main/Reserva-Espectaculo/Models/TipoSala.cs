using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class TipoSala
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [Range(0.0D, int.MaxValue, ErrorMessage = ErrorMessage.PositiveNumber)]
        public float Precio { get; set; }
        public List<Sala> Salas { get; set; }

        public TipoSala()
        {

        }
        public TipoSala(string nombre, float precio)
        {
            this.Nombre = nombre;
            this.Precio = precio;
        }
    }
}
