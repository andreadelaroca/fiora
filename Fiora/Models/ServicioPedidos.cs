using Fiora.Models;
public class ServicioPedido
{
    public List<string> ValidarPedido(SolicitudPedido pedido)
    {
        var errores = new List<string>();

        if (pedido.articulos == null || !pedido.articulos.Any())
        {
            errores.Add("El pedido no puede estar vacío.");
            return errores;
        }

        double total = 0;
        foreach (var articulo in pedido.articulos)
        {
            // Validar de integridad
            if (articulo.MontoPedido <= 0) errores.Add($"Precio inválido para producto {articulo.Id}");

            total += Convert.ToDouble(articulo.CantidadItems) * articulo.MontoPedido;
        }

        // Regla de negocio: Pedido máximo
        if (total > 5000) errores.Add("El pedido requiere autorización de un gerente (Total > 5000).");

        return errores;
    }

    public bool CrearPedido(SolicitudPedido pedido)
    {
        var errores = ValidarPedido(pedido);
        if (errores.Any()) return false;

        // To do: Añadir código para guardar en una base de datos real
        return true;
    }
}