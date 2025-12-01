namespace Fiora.Models
{
    // Ítem tomado del inventario directamente a un pedido
    public class PedidoInventario
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public int InventarioId { get; set; }
        public Inventario Inventario { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
    }
}