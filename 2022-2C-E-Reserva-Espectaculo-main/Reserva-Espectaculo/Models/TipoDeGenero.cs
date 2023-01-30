using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Reserva_Espectaculo.Models
{
    public enum TipoDeGenero
    {
        
        SUSPENSO,
        ACCION,
        TERROR,
        COMEDIA,
        ROMANTICA,
        DRAMA,
        [Display(Name = "INFANTIL")]
        PARA_NIÑOS,
    }
}
