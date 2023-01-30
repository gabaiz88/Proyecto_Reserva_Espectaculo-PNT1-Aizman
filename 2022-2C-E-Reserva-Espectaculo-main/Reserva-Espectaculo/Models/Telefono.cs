using System;
using System.ComponentModel.DataAnnotations;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Telefono
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessage.PhoneFormat)]
        public string CodArea { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessage.PhoneFormat)]
        public string Numero { get; set; }

        public int PersonaId { get; set; }

        public Persona Persona{ get; set; }

        public Telefono()
        {
        }

        public Telefono(int id, string codArea, string numero, int personaId)
        {
            this.Id = id;   
            this.CodArea = codArea;
            this.Numero = numero;   
            this.PersonaId = personaId;
        }
        
    
    }
}