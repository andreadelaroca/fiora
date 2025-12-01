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

    public enum ModoPago
    {
        Transferencia = 0,
        Efectivo = 1,
        Tarjeta = 2
    }

    public class Pedido
    {
        public int Id { get; set; }
        [Required, MaxLength(80)]
        public string OcasionPedido { get; set; } = null!;

        // Relación con Cliente (requerida)
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // Datos de contacto/envío y snapshot del nombre
        [Required, MaxLength(120)]
        public string NombreCliente { get; set; } = null!;

        [Required, MaxLength(200)]
        public string DireccionEnvio { get; set; } = null!;

        // Mensaje opcional de dedicatoria
        public string? MensajePedido { get; set; }

        // Modo de pago con enum tipado
        public ModoPago ModoPago { get; set; } = ModoPago.Transferencia;

        // Dinero: decimal recomendado
        public decimal MontoTotal { get; set; }

        public EstadoPedido EstadoPedido { get; set; } = EstadoPedido.Pendiente;

        // Si hay servicio en sitio; si es falso, los siguientes valores son null
        public bool Servicio { get; set; }
        public DateTime? FechaHoraEntrega { get; set; }
        public string? DireccionEvento { get; set; }
        public string? TematicaEvento { get; set; }
        public string? ColoresEvento { get; set; }

        // Relación con Admin (opcional si aún no asignado)
        public int? AdminId { get; set; }
        public Admin? Admin { get; set; }

        // Relación con Arreglo (obligatorio: lo que se está pidiendo)
        public int ArregloId { get; set; }
        public Arreglo Arreglo { get; set; } = null!;

        // Ítems de inventario agregados directamente al pedido
        public ICollection<PedidoInventario> ItemsInventario { get; set; } = new List<PedidoInventario>();

        public Pedido()
        {

        }
    }
}
