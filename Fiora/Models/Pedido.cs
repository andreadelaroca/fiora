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
            public string OcasionPedido { get; set; } = null!;
            public string TipoArreglo { get; set; } = null!;
        public int IdCliente { get; set; }
            public string NombreCliente { get; set; } = null!;
            public string DireccionEnvio { get; set; } = null!;
            public string MensajePedido { get; set; } = null!;
            public string ModoPago { get; set; } = null!;
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

            // Relación con Arreglo (obligatorio: lo que se está pidiendo)
            public int ArregloId { get; set; }
            public Arreglo Arreglo { get; set; } = null!;

        public Pedido()
        {

        }
    }
}
