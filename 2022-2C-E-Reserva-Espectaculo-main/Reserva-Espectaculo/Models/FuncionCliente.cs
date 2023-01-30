using System.ComponentModel.DataAnnotations;

namespace Reserva_Espectaculo.Models
{
    public class FuncionCliente
    {
        public Funcion Funcion { get; set; }
        public Cliente Cliente { get; set; }
        [Key]
        public int FuncionId { get; set; }
        [Key]
        public int ClienteId { get; set; }

        public FuncionCliente()
        {

        }
        public FuncionCliente(int funcionId, int clienteId)
        {
            this.FuncionId = funcionId;
            this.ClienteId = clienteId;
        }

    }
}
