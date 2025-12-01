using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiora.Models
{
    public enum TemporadaArreglo
    {
        Primavera = 1,
        Verano = 2,
        Otonio = 3,
        Invierno = 4
    }

    public enum OcasionArreglo
    {
        General = 0,
        Cumpleanios = 1,
        Aniversario = 2,
        Boda = 3,
        Graduacion = 4,
        SanValentin = 5,
        DiaMadre = 6,
        Navidad = 7
    }

    public class Arreglo
    {
        public int Id { get; set; }

        public TemporadaArreglo TemporadaArreglo { get; set; }
        public OcasionArreglo OcasionArreglo { get; set; }

        [Required, MaxLength(100)]
        public string NombreArreglo { get; set; } = null!;

        [Required, MaxLength(80)]
        public string TipoArreglo { get; set; } = null!;

        [Range(0, 100000)]
        public decimal PrecioArreglo { get; set; }

        [Range(0, 240)] // horas
        public int TiempoEstimadoHoras { get; set; }

        // Disponibilidad calculada según la temporada actual (no se persiste directamente)
        [NotMapped]
        public bool Disponible => TemporadaArreglo == ObtenerTemporadaActual();

        private TemporadaArreglo ObtenerTemporadaActual()
        {
            int mes = DateTime.UtcNow.Month;
            return mes switch
            {
                3 or 4 or 5 => TemporadaArreglo.Primavera,
                6 or 7 or 8 => TemporadaArreglo.Verano,
                9 or 10 or 11 => TemporadaArreglo.Otonio,
                _ => TemporadaArreglo.Invierno
            };
        }
    }
}
