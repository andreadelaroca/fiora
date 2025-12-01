using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fiora.Models
{
    public enum EstadoCliente
    {
        Inactivo = 0,
        Activo = 1
    }

    public class Cliente
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NombreCliente { get; set; } = null!;

        [Required, EmailAddress, MaxLength(256)]
        public string CorreoCliente { get; set; } = null!;

        [Required, MaxLength(200)]
        public string PasswordCliente { get; set; } = null!;

        [Required, MaxLength(20)]
        public string TelefonoCliente { get; set; } = null!;

        [Required, MaxLength(200)]
        public string DireccionCliente { get; set; } = null!;

        public DateTime FechaRegistroCliente { get; set; } = DateTime.UtcNow;

        public EstadoCliente Estado { get; set; } = EstadoCliente.Activo;

        // Navegación: todos los pedidos del cliente
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        // Vistas calculadas (no mapeadas) para separar actuales e historial
        [NotMapped]
        public IEnumerable<Pedido> PedidosActuales =>
            Pedidos.Where(p => p.EstadoPedido == EstadoPedido.Pendiente || p.EstadoPedido == EstadoPedido.EnProceso);

        [NotMapped]
        public IEnumerable<Pedido> HistorialPedidos =>
            Pedidos.Where(p => p.EstadoPedido == EstadoPedido.Entregado || p.EstadoPedido == EstadoPedido.Cancelado);
    }
}
