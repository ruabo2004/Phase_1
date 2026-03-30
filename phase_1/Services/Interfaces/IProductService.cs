using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using phase_1.Models;

namespace phase_1.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetTop3HighestPricedProductsByCategoryAsync(string category);
        Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);
        Task<Product> CreateProductAsync(Product newProduct);
        Task<Product> UpdateProductAsync(Product newProduct);
    }
}
