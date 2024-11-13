using Hands_on2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hands_on2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinearRegressionController : ControllerBase
    {
        private readonly ILinearRegressionService _linearRegressionService;
        public LinearRegressionController(ILinearRegressionService linearRegressionService)
        {
            _linearRegressionService = linearRegressionService;
        }

        [HttpPost("SimpleLinearRegression")]
        public IActionResult SimpleLinearRegression(IFormFile excelFile) 
        {

            var sales = _linearRegressionService.ReadExcel(excelFile);
            var modelRegression = _linearRegressionService.CalcularModeloRegresion(sales);

            Console.WriteLine($"Beta0: {modelRegression.Beta0}");
            Console.WriteLine($"Beta1: {modelRegression.Beta1}");
            return Ok(modelRegression);
        }
    }
}
