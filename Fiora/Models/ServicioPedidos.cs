using Fiora.Models;
using Fiora.Data;
using Microsoft.EntityFrameworkCore;

public class ServicioPedido
{
    private readonly ApplicationDbContext _db;

    public ServicioPedido(ApplicationDbContext db)
    {
        _db = db;
    }

    public List<string> ValidarPedido(SolicitudPedido req)
    {
        var errores = new List<string>();

        if (req.clienteId <= 0) errores.Add("Cliente inválido.");
        if (string.IsNullOrWhiteSpace(req.ocasionPedido)) errores.Add("La ocasión del pedido es requerida.");
        if (string.IsNullOrWhiteSpace(req.nombreCliente)) errores.Add("El nombre del cliente es requerido.");
        if (string.IsNullOrWhiteSpace(req.direccionEnvio)) errores.Add("La dirección de envío es requerida.");

        // Validar arreglo
        var arreglo = _db.Arreglo
            .Include(a => a.Componentes)
                .ThenInclude(c => c.Inventario)
            .FirstOrDefault(a => a.Id == req.arregloId);
        if (arreglo == null) errores.Add("El arreglo seleccionado no existe.");
        else if (!arreglo.Disponible) errores.Add("El arreglo no está disponible (temporada o inventario insuficiente).");

        // Validar servicio en sitio
        if (req.servicio)
        {
            if (req.fechaHoraEntrega == null) errores.Add("La fecha y hora de entrega es requerida para servicio en sitio.");
            if (string.IsNullOrWhiteSpace(req.direccionEvento)) errores.Add("La dirección del evento es requerida para servicio en sitio.");
        }

        // Validar items directos de inventario
        if (req.itemsInventario != null)
        {
            foreach (var item in req.itemsInventario)
            {
                if (item.cantidad <= 0) { errores.Add($"Cantidad inválida para inventario {item.inventarioId}."); continue; }

                var inv = _db.Inventario.FirstOrDefault(i => i.Id == item.inventarioId);
                if (inv == null) { errores.Add($"Ítem de inventario {item.inventarioId} no existe."); continue; }
                if (inv.Cantidad < item.cantidad) errores.Add($"Stock insuficiente para '{inv.NombreProducto}'. Disponible: {inv.Cantidad}, requerido: {item.cantidad}.");
            }
        }

        return errores;
    }

    public (bool ok, List<string> errores, int? pedidoId) CrearPedido(SolicitudPedido req)
    {
        var errores = ValidarPedido(req);
        if (errores.Any()) return (false, errores, null);

        // Cargar arreglo con precio
        var arreglo = _db.Arreglo.First(a => a.Id == req.arregloId);

        // Calcular total: precio del arreglo + ítems de inventario (si traen precio)
        decimal total = arreglo.PrecioArreglo;
        if (req.itemsInventario != null)
        {
            foreach (var item in req.itemsInventario)
            {
                total += (item.precioUnitario ?? 0m) * item.cantidad;
            }
        }

        // Regla de negocio: tope requiere autorización
        if (total > 5000m) errores.Add("El pedido requiere autorización de un gerente (Total > 5000).");
        if (errores.Any()) return (false, errores, null);

        var pedido = new Pedido
        {
            OcasionPedido = req.ocasionPedido,
            ClienteId = req.clienteId,
            NombreCliente = req.nombreCliente,
            DireccionEnvio = req.direccionEnvio,
            MensajePedido = req.mensajePedido,
            ModoPago = req.modoPago,
            MontoTotal = total,
            EstadoPedido = EstadoPedido.Pendiente,
            Servicio = req.servicio,
            FechaHoraEntrega = req.servicio ? req.fechaHoraEntrega : null,
            DireccionEvento = req.servicio ? req.direccionEvento : null,
            TematicaEvento = req.servicio ? req.tematicaEvento : null,
            ColoresEvento = req.servicio ? req.coloresEvento : null,
            ArregloId = req.arregloId
        };

        // Mapear ítems de inventario directos
        if (req.itemsInventario != null && req.itemsInventario.Count > 0)
        {
            pedido.ItemsInventario = req.itemsInventario.Select(x => new PedidoInventario
            {
                InventarioId = x.inventarioId,
                Cantidad = x.cantidad,
                PrecioUnitario = x.precioUnitario
            }).ToList();
        }

        _db.Pedido.Add(pedido);
        _db.SaveChanges();

        return (true, new List<string>(), pedido.Id);
    }
}