using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiora.Models
{
    public class Inventario
    {
        public int Id { get; set; }

        [Required]
        public int CodigoItem { get; set; }

        [Required, MaxLength(120)]
        public string NombreProducto { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; }

        // Disponibilidad calculada a partir de la cantidad
        [NotMapped]
        public bool Disponible => Cantidad > 0;

        // Relaciones
        public ICollection<ArregloInventario> UsosEnArreglos { get; set; } = new List<ArregloInventario>();
        public ICollection<PedidoInventario> UsosEnPedidos { get; set; } = new List<PedidoInventario>();
    }
}
