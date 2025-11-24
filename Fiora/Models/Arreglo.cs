namespace Fiora.Models
{
    public class Arreglo
    {
        public int Id { get; set; }
        public string TemporadaArreglo { get; set; }
        public string OcasionArreglo { get; set; }
        public string NombreArreglo { get; set; }
        public string TipoArreglo { get; set; }
        public double PrecioArreglo { get; set; }
        public double TiempoEstimadoHoras { get; set; }
        public bool DisponibilidadArreglo { get; set; } // debe comparar con la temporada a la que pertenece y la actual

        public Arreglo() { }

    }
}
