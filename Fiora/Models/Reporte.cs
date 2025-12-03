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
       
        public TipoReporte Tipo { get; set; } = TipoReporte.Ventas;

        //-----------------------------------------
        //Periodo si es diario, semanal, mensual
        [Required]
        public PeriodoReporte Periodo { get; set; } = PeriodoReporte.Rango;

        //rango de fechas del reporte

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        //Datos del reporte (RF09)
        [Display(Name = "Total del pedidos")]
        public int TotalPedidos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total de ventas")]
        public decimal TotalVentas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TicketPromedio { get; set; }

        public int PedidosCompletados { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosCancelados { get; set; }
    }
}
