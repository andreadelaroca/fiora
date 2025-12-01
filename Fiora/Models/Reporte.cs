using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiora.Models
{
    public enum TipoReporte
    {
        Ventas = 0
    }

    public enum PeriodoReporte
    {
        Diario = 0,
        Semanal = 1,
        Mensual = 2,
        Rango = 3
    }

    // Reporte financiero persistido con métricas agregadas
    public class Reporte
    {
        public int Id { get; set; }

        [Required]
        public TipoReporte Tipo { get; set; } = TipoReporte.Ventas;

        [Required]
        public PeriodoReporte Periodo { get; set; } = PeriodoReporte.Rango;

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public int TotalPedidos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVentas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TicketPromedio { get; set; }

        public int PedidosCompletados { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosCancelados { get; set; }
    }
}
