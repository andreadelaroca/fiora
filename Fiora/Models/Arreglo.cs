using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        // Disponibilidad calculada según temporada e inventario (no se persiste)
        [NotMapped]
        public bool Disponible => EsTemporadaActual() && TieneInventarioSuficiente();

        private bool EsTemporadaActual() => TemporadaArreglo == ObtenerTemporadaActual();

        private bool TieneInventarioSuficiente()
        {
            if (Componentes == null || Componentes.Count == 0) return true; // sin componentes específicos
            return Componentes.All(c => c != null && c.CantidadNecesaria > 0 && c.Inventario != null && c.Inventario.Cantidad >= c.CantidadNecesaria);
        }

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

        // Componentes de inventario requeridos por este arreglo
        public ICollection<ArregloInventario> Componentes { get; set; } = new List<ArregloInventario>();
    }
}
