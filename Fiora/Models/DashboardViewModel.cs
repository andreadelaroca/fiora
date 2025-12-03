using System;
using System.Collections.Generic;
using Fiora.Models;

namespace Fiora.Models
{
    public class DashboardTotals
    {
        public int ClientesTotal { get; set; }
        public int InventarioItems { get; set; }
        public int PedidosActivos { get; set; }
        public int EntregasHoy { get; set; }
        public decimal VentasMes { get; set; } // placeholder
    }

    public class NotificationItem
    {
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
    }

    public class DashboardViewModel
    {
        public DashboardTotals Totals { get; set; } = new DashboardTotals();
        public IReadOnlyList<Cliente> RecentClients { get; set; } = Array.Empty<Cliente>();
        public IReadOnlyList<Inventario> LowStockItems { get; set; } = Array.Empty<Inventario>();
        public IReadOnlyList<Pedido> UpcomingDeliveries { get; set; } = Array.Empty<Pedido>();
        public IReadOnlyList<NotificationItem> Notifications { get; set; } = Array.Empty<NotificationItem>();
    }
}