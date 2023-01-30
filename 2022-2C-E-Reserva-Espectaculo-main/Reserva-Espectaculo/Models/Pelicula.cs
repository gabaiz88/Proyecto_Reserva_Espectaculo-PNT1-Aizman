using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Titulo { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        public DateTime FechaLanzamiento { get; set; }
        
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(500, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Descripcion { get; set; }
        public TipoDeGenero Genero { get; set; }
        public List<Funcion> Funciones {  get; set; }

        public string Foto { get; set; }

        public Pelicula()
        {

        }
        public Pelicula(DateTime fechaLanzamiento, string titulo, string descripcion, TipoDeGenero genero, String foto)
        {
            this.FechaLanzamiento = fechaLanzamiento;
            this.Titulo = titulo;
            this.Descripcion = descripcion;
            this.Genero = genero;
            this.Foto = foto != null ? foto : Configs.FotoDefaultPelicula;
        }
    }
}
