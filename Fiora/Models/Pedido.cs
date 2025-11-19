namespace Fiora.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public List<string> TipoPedido { get; set; } = new List<string>(2); //TipoServicio, TipoArreglo
        public List<int> IdItems { get; set; } = new List<int>(); // Lista de IDs de items en el pedido
        public List<int> CantidadItems { get; set; } = new List<int>(); // Cantidad de cada item en el pedido
        public List<Inventario> ItemsPedido { get; set; } = new List<Inventario>(); // Lista de items en el pedido
        public int IdCliente { get; set; } // Clave foránea para la relación con Cliente
        public Cliente Cliente { get; set; } // Propiedad de navegación hacia Cliente
        public int IdAdmin { get; set; } // Clave foránea para la relación con Admin
        public string EncargadoPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; } // Fecha estimada de entrega
        public decimal MontoTotal { get; set; }
        public List<string> EstadoPedido { get; set; } = new List<string>(3); //EstadoPendiente, EstadoEnviado, EstadoEntregado
        public List<string> MetodoPago { get; set; } = new List<string>(3); //PagoTarjetaCredito, PagoTransferencia, PagoEfectivo
        public List<string> Disponibilidad { get; set; } = new List<string>(2); //Disponible, NoDisponible

        public Pedido() // constructor vacío
        {

        }
    }
}
