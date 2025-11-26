using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Fiora.Models
{
    public class Reporte
    {
<<<<<<< HEAD
        // arbol de búsqueda para encontrar el item más utilizado
=======
        [Key]
        public int Id { get; set; }

        //Tipo de ventas o inventarios
        [Required]
        [Display(Name = "Tipo de Reporte")]
        public string Tipo { get; set; } = string.Empty;

        //-----------------------------------------
        //Periodo si es diario, semanal, mensual
        [Required]
        [Display(Name = "Periodo")]
        public string Periodo { get; set; } = string.Empty;
        //-------------------------------------------------

        //------------------------------------------
        //rango de fechas del reportes
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        //-------------------------------------------

        //------------------------------------------
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime Fechafin { get; set; }
        //------------------------------------------



        //------------------------------------------
        //Datos del reporte (RF09)
        [DisplayName("Total del pedidos")]
        public int TotalPedidos { get; set; }
        //------------------------------------------


        //------------------------------------------
        [DisplayName(nameof = "Total de ventas")]
        public decimal TotalVentas { get; set; }
        //------------------------------------------
         

        //------------------------------------------
        [DisplayName("Total de productos")]
        public int TotalProductos { get; set; }
        //------------------------------------------

        //------------------------------------------
        [DisplayName("Productos bajo stock")]
        public int ProductosBajoStock { get; set; }


>>>>>>> 3dd4d2dc765fedcc5592a41da8d98bc50fb64972
    }
}
