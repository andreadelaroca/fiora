namespace Fiora.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string NombreAdmin { get; set; }
        public string CorreoAdmin { get; set; }
        public string PasswordAdmin { get; set; }
        public List<Pedido> PedidosAdmin { get; set; } = new List<Pedido>();
        public Stack<Pedido> HistorialAdmin { get; set; } = new Stack<Pedido>();
        public List<string> EstadoAdmin { get; set; } = new List<string>(2); //EstadoActivo, EstadoInactivo
        public List<string> RolAdmin { get; set; } = new List<string>(2); //RolGerente, RolEmpleado

        public Admin()
        {

        }
    }
}
