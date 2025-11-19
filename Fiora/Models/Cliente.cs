namespace Fiora.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public string CorreoCliente { get; set; }
        public string TelefonoCliente { get; set; }

        public int List<Pedido> Pedidos { get; set; } // Relación uno a muchos con la entidad Pedido
        // TODO arreglar esta lista

        public Cliente()
        {

        }
    }
}
