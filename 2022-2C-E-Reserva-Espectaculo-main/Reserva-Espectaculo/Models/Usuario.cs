using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Usuario
    {
        [Key, ForeignKey("Persona")]
        public int Id { get; protected set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(12, MinimumLength = 8, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Username { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; private set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public Persona Persona { get; set; }

        public Usuario()
        {
        }

        public Usuario(int Id, string Username, string Email, string Password)
        {
            this.Id = Id;
            this.Username = Username;
            this.Email = Email;
            this.Password = Password;
        }

    }
}
