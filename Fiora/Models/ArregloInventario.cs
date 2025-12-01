namespace Fiora.Models
{
    // Entidad de unión: qué y cuánto inventario requiere un arreglo
    public class ArregloInventario
    {
        public int Id { get; set; }
        public int ArregloId { get; set; }
        public Arreglo Arreglo { get; set; } = null!;
        public int InventarioId { get; set; }
        public Inventario Inventario { get; set; } = null!;
        public int CantidadNecesaria { get; set; }
    }
}