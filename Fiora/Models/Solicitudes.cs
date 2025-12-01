using Fiora.Models;

public class SolicitudInicioSesion
{
    public string usuario { get; set; }
    public string contrasena { get; set; }
}

public class SolicitudRegistro
{
    public string correo { get; set; }
    public string contrasena { get; set; }
    public string usuario { get; set; }
}

public class SolicitudItemInventario
{
    public int inventarioId { get; set; }
    public int cantidad { get; set; }
    public decimal? precioUnitario { get; set; }
}

// Estructura para crear un Pedido
public class SolicitudPedido
{
    public int clienteId { get; set; }
    public int arregloId { get; set; }
    public string ocasionPedido { get; set; }
    public string nombreCliente { get; set; }
    public string direccionEnvio { get; set; }
    public string? mensajePedido { get; set; }
    public ModoPago modoPago { get; set; }

    public bool servicio { get; set; }
    public DateTime? fechaHoraEntrega { get; set; }
    public string? direccionEvento { get; set; }
    public string? tematicaEvento { get; set; }
    public string? coloresEvento { get; set; }

    public List<SolicitudItemInventario> itemsInventario { get; set; } = new();
}