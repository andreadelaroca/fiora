using Azure.Core;
using Fiora.Models;
using Microsoft.AspNetCore.Identity.Data;

public class InicioSesion
{
    // TO DO: Cambiarlo a la clase "Usuario" cuando se implemente.
    private static List<Cliente> usuarios = new List<Cliente>();

    public bool Registrar(SolicitudRegistro solicitud)
    {
        // TO DO: Validar si el usuario ya existe para evitar enumeración

        usuarios.Add(new Cliente());    
        // TO DO: Añadir los valores de usuarios a la llamada del constructor de cliente

        return true;
    }

    public string IniciarSesion(SolicitudInicioSesion solicitud)
    {
        var user = usuarios.FirstOrDefault(u => u.NombreCliente == solicitud.usuario);
        if (user == null)
            return null;

        if (user.PasswordCliente == solicitud.contrasena)
        {
            return "token_jwt"; // To Do: Añadir el token/verificante de la autenticación
        }
        return null;
    }
}