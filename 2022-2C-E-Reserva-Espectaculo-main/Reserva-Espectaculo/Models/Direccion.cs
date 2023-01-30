using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Direccion
    {
        [Key, ForeignKey("Persona")]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Calle { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [Range(1, 9999, ErrorMessage = ErrorMessage.MaxInt)]
        public int Altura { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.PostalCode)]
        public int CodigoPostal { get; set; }

        public Persona Persona { get; set; }

        public Direccion()
        {

        }
        public Direccion(string calle, int altura, string ciudad, int codigoPostal)
        {
            this.Calle = calle;
            this.Altura = altura;
            this.Ciudad = ciudad;
            this.CodigoPostal = codigoPostal;
        }

    }
}
