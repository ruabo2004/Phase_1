using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.Models;

namespace phase_1.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetTop3HighestPricedProductsByCategory(string category);
        
        IEnumerable<CategoryTotal> GetTotalPriceByCategory();
        
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);
    }
}
