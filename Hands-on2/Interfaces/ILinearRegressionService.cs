using Hands_on2.Models;

namespace Hands_on2.Interfaces
{
    public interface ILinearRegressionService
    {
        List<SalesModel> ReadExcel(IFormFile excelFile);
        RegressionModel CalcularModeloRegresion(List<SalesModel> data);
    }
}
