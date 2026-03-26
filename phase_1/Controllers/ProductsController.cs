using Microsoft.AspNetCore.Mvc;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;

namespace phase_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("top3/{category}")]
        public IActionResult GetTop3ByCategory(string category)
        {
            var result = _productService.GetTop3HighestPricedProductsByCategory(category);
            return Ok(result);
        }

        [HttpGet("summary")]
        public IActionResult GetCategorySummary()
        {
            var result = _productService.GetTotalPriceByCategory();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProductsAsync([FromQuery] string keyword)
        {
            var result = await _productService.SearchProductsAsync(keyword);
            return Ok(result);
        }
    }
}
