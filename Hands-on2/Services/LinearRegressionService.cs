using Hands_on2.Interfaces;
using Hands_on2.Models;
using SpreadsheetLight;

namespace Hands_on2.Services
{
    public class LinearRegressionService : ILinearRegressionService
    {
        public RegressionModel CalcularModeloRegresion(List<SalesModel> data)
        {
            int n = data.Count;
            if (n == 0)
            {
                throw new ArgumentException("La lista de datos está vacía.");
            }
            // Cálculo de las medias
            decimal sumX = data.Sum(d => d.Advertising);
            decimal sumY = data.Sum(d => d.Sales);
            decimal promedioX = sumX / n;
            decimal promedioY = sumY / n;

            // Cálculo de Beta1
            decimal sumNumerador = data.Sum(d => (d.Advertising - promedioX) * (d.Sales - promedioY));
            decimal sumDenominador = data.Sum(d => (d.Advertising - promedioX) * (d.Advertising - promedioX));
            decimal beta1 = sumDenominador != 0 ? sumNumerador / sumDenominador : 0;

            // Cálculo de Beta0
            decimal beta0 = promedioY - beta1 * promedioX;

            return new RegressionModel { Beta0 = beta0, Beta1 = beta1 };
        }

        public List<SalesModel> ReadExcel(IFormFile archivoExcel)
        {
            var listaSales = new List<SalesModel>();

            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                throw new ArgumentException("El archivo no fue proporcionado o está vacío.");
            }

            using(var stream = new MemoryStream())
            {
                archivoExcel.CopyTo(stream);
                stream.Position = 0;

                using (var documento = new SLDocument(stream))
                {
                    // Selecciona la primera hoja
                    documento.SelectWorksheet("Hoja1");

                    int fila = 2; // Asume que los encabezados están en la primera fila
                    while (!string.IsNullOrEmpty(documento.GetCellValueAsString(fila, 1))) // Lee hasta una fila vacía
                    {
                        var modelo = new SalesModel
                        {
                            Year = documento.GetCellValueAsInt32(fila, 1),
                            Sales = documento.GetCellValueAsDecimal(fila, 2),
                            Advertising = documento.GetCellValueAsDecimal(fila, 3)
                        };

                        listaSales.Add(modelo);
                        fila++;
                    }
                }
                return listaSales;
            }
        }
    }
}
