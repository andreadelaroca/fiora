using Fiora.Models;
public class SolicitudInicioSesion
{
    // Campo requerido
    public string usuario { get; set; }

    // Lo mismo
    public string contrasena { get; set; }
}

public class SolicitudRegistro
{
    // Dirección de correo
    public string correo { get; set; }

    // Contraseña
    public string contrasena { get; set; }

    // Usuario
    public string usuario { get; set; }
}

// Para el Pedido
public class SolicitudPedido
{
    public int idCliente { get; set; }
    public List<Pedido> articulos { get; set; }
}