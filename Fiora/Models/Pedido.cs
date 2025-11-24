namespace Fiora.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string OcasionPedido { get; set; }
        public string TipoArreglo { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionEnvio { get; set; }
        public string MensajePedido { get; set; }
        public string ModoPago { get; set; }
        public double MontoTotal { get; set; }
        public string EstadoPedido { get; set; }
        public bool Servicio { get; set; } // si es falso los atributos siguientes son null
        public DateTime FechaHoraEntrega { get; set; }
        public string? DireccionEvento { get; set; }
        public string? TematicaEvento { get; set; }
        public string? ColoresEvento { get; set; }

        public Pedido()
        {

        }
    }
}
