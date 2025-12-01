using System.ComponentModel.DataAnnotations;

namespace Fiora.Models
{
    public enum EstadoPedido
    {
        Pendiente = 0,
        EnProceso = 1,
        Entregado = 2,
        Cancelado = 3
    }

    public class Pedido
    {
        public int Id { get; set; }
        public string OcasionPedido { get; set; }
        public string TipoArreglo { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionEnvio { get; set; }
        public string MensajePedido { get; set; }
        public string ModoPago { get; set; }
        public double MontoTotal { get; set; }

        public EstadoPedido EstadoPedido { get; set; } = EstadoPedido.Pendiente;

        public bool Servicio { get; set; } // si es falso los atributos siguientes son null
        public DateTime FechaHoraEntrega { get; set; }
        public string? DireccionEvento { get; set; }
        public string? TematicaEvento { get; set; }
        public string? ColoresEvento { get; set; }

        // Relación con Admin (opcional si aún no asignado)
        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }

        public Pedido()
        {

        }
    }
}
