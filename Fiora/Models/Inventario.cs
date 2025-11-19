namespace Fiora.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public int CodigoItem { get; set; }
        public string NombreItem { get; set; }
        public string CantidadItem { get; set; }

        public Inventario()
        {

        }
    }
}
