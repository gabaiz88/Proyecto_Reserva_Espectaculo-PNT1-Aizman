using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Funcion
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public DateTime FechaYHora { get; set; }
      
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public string Descripcion { get; set; }
        // [Required(ErrorMessage = ErrorMessage.Requerido)]
        public int ButacasDisponibles { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public bool Confirmada { get; set; } = false;
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public int Duracion { get;private set; } = 2;
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }
        public int SalaId { get; set; }
        public Sala Sala { get; set; }
        public List<Reserva> Reservas  { get; set; }
        public List<FuncionCliente> ClientesFuncion { get; set; }

        public Funcion()
        {

        }
        public Funcion(DateTime fecha, string descripcion, bool confirmada, int peliculaId, int salaId)
        {
            this.FechaYHora = fecha;
            this.Descripcion = descripcion;
            this.Confirmada = confirmada;
            this.Duracion = 2;
            this.PeliculaId = peliculaId;
            this.SalaId = salaId;
            this.ButacasDisponibles = 220;
        }
    }
}
