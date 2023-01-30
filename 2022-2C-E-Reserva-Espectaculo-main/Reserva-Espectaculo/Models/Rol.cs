using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Reserva_Espectaculo.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Reserva_Espectaculo.Models
{
    public class Rol : IdentityRole<int>
    {

        //public int Id { get; set; }
        
        public Rol() : base()
        {

        }

        public Rol(string name) : base(name) { }

        [Display(Name = Alias.RoleName)]
        public string Name
        {
            get { return base.Name; }
            set { base.Name = value; }

        }

        public override string NormalizedName { 
            get => base.NormalizedName;
            set => base.NormalizedName = value;}
    }

}
