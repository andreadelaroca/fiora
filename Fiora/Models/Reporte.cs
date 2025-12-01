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
<<<<<<< HEAD
        [Display(Name = "Tipo de Reporte")]
        public string Tipo { get; set; } = string.Empty;
        //-----------------------------------------
        //Periodo si es diario, semanal, mensual
        [Required]
        [Display(Name = "Periodo")]
        public string Periodo { get; set; } = string.Empty;


        //rango de fechas del reportes
=======
        public TipoReporte Tipo { get; set; } = TipoReporte.Ventas;

        [Required]
        public PeriodoReporte Periodo { get; set; } = PeriodoReporte.Rango;

>>>>>>> d139e22ed61d94d51120dcd60c8fd5f35dd93f3e
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
<<<<<<< HEAD
      
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime Fechafin { get; set; }
     

        //Datos del reporte (RF09)
        [DisplayName("Total del pedidos")]
        public int TotalPedidos { get; set; }
     

        [DisplayName(nameofxx = "Total de ventas")]
        public decimal TotalVentas { get; set; }


        [DisplayName("Total de productos")]
        public int TotalProductos { get; set; }


        [DisplayName("Productos bajo stock")]
        public int ProductosBajoStock { get; set; }
=======

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
>>>>>>> d139e22ed61d94d51120dcd60c8fd5f35dd93f3e
    }
}
