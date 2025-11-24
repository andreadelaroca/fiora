namespace Fiora.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public int CodigoItem { get; set; }
        public IProducto Producto { get; set; }
        public int Cantidad { get; set; }
        public bool Disponiblé { get; set; }

        public Inventario()
        {

        }
    }
}
