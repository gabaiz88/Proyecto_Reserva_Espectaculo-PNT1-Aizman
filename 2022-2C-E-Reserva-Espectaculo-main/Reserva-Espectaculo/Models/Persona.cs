using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Reserva_Espectaculo.Helpers;

namespace Reserva_Espectaculo.Models
{
    public class Persona : IdentityUser<int>
    {
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessage.MinMaxString)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [Range(1, 99999999, ErrorMessage = ErrorMessage.MaxInt)]
        public int Dni { get; set; }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.EmailAddress)]
        public override string Email {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [Required(ErrorMessage = ErrorMessage.Requerido)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrorMessage.DateFormat)]
        public int Telefono { get; set; }
        [DataType(DataType.Date, ErrorMessage = ErrorMessage.PhoneFormat)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public Direccion Direccion { get; set; }

        public List<Telefono> Telefonos { get; set; }

        public Usuario Usuario { get; set; }

        public Persona()
        {

        }

        public Persona(string nombre, string apellido, int dni, string email, int telefono, string calle, int altura, string ciudad)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Email = email;
            UserName = email;
            Telefono = telefono;
            new Direccion(calle, altura, ciudad, 1040);
        }
    }
}
