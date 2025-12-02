namespace Fiora.Models
{
    public class ArregloFabrica
    {
        public Arreglo CrearArreglo(TipoArregloEnum tipoSeleccionado)
        {
            var arreglo = new Arreglo
            {
                TipoArreglo = tipoSeleccionado
            };

            switch (tipoSeleccionado)
            {
                case TipoArregloEnum.CorTradicion:
                    arreglo.EstablecerVariables("Corona Fúnebre Tradicional", 125.00m, 90, OcasionArreglo.Funeral);
                    break;
                case TipoArregloEnum.CorCorazon:
                    arreglo.EstablecerVariables("Corona de corazón", 125.00m, 60 * 3, OcasionArreglo.Funeral);
                    break;
                case TipoArregloEnum.CorMediaLuna:
                    arreglo.EstablecerVariables("Corona de media luna", 125.00m, 60 * 3, OcasionArreglo.Funeral);
                    break;
                case TipoArregloEnum.CorCustom:
                    arreglo.EstablecerVariables("Corona diseño de cliente", 125.00m, 60 * 3, OcasionArreglo.Funeral);
                    break;
                case TipoArregloEnum.DocRosas:
                    arreglo.EstablecerVariables("Docena de rosas", 45.00m, 60, OcasionArreglo.Cumpleanios);
                    break;

                // ... Agrega el resto de los 'case' aquí ...

                default:
                    arreglo.NombreArreglo = "Arreglo Personalizado";
                    break;
            }

            return arreglo;
        }
    }
}
