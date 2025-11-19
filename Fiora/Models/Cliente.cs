namespace Fiora.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public string CorreoCliente { get; set; }
        public string PasswordCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string DireccionCliente { get; set; }
        public DateTime FechaRegistroCliente { get; set; }
        public List<Pedido> PedidosCliente { get; set; } = new List<Pedido>();
        public Stack<Pedido> HistorialPedidos { get; set; } = new Stack<Pedido>();
        public List<string> EstadoCliente { get; set; } = new List<string>(2); //EstadoActivo, EstadoInactivo
        public Cliente()
        {

        }
    }
}
