using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fiora.Models
{
    public enum EstadoAdmin
    {
        Inactivo = 0,
        Activo = 1
    }

    public class Admin
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string NombreAdmin { get; set; } = null!;

        [Required, EmailAddress, MaxLength(256)]
        public string CorreoAdmin { get; set; } = null!;

        [Required, MaxLength(200)]
        public string PasswordAdmin { get; set; } = null!;

        public EstadoAdmin Estado { get; set; } = EstadoAdmin.Activo;

        // Navegación: todos los pedidos asociados al admin
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        // Vistas calculadas (no mapeadas) para separar actuales e historial
        [NotMapped]
        public IEnumerable<Pedido> PedidosActuales =>
            Pedidos.Where(p => p.EstadoPedido == Fiora.Models.EstadoPedido.Pendiente || p.EstadoPedido == Fiora.Models.EstadoPedido.EnProceso);

        [NotMapped]
        public IEnumerable<Pedido> HistorialPedidos =>
            Pedidos.Where(p => p.EstadoPedido == Fiora.Models.EstadoPedido.Entregado || p.EstadoPedido == Fiora.Models.EstadoPedido.Cancelado);
    }
}
