using Microsoft.AspNetCore.Mvc;
using phase_1.Models;
using phase_1.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetTop3ByCategoryAsync([FromRoute] string category)
        {
            var result = await _productService.GetTop3HighestPricedProductsByCategoryAsync(category);
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetCategorySummaryAsync()
        {
            var result = await _productService.GetTotalPriceByCategoryAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProductsAsync([FromQuery] string keyword)
        {
            var result = await _productService.SearchProductsAsync(keyword);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
        {
            var result = await _productService.CreateProductAsync(product);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] Product product)
        {
            var result = await _productService.UpdateProductAsync(product);
            if (result == null)
            {
                return NotFound($"Khong thay san pham co Id = {product.Id}");
            }
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result == null)
            {
                return NotFound($"Khong thay san pham co Id = {id}");
            }
            return Ok(result);
        }
    } 
}
