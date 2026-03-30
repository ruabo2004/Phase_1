using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;
using phase_1.Services.Interfaces;

namespace phase_1.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetTop3HighestPricedProductsByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .OrderByDescending(p => p.Price)
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryTotal>> GetTotalPriceByCategoryAsync()
        {
            return await _context.Products
                .GroupBy(p => p.Category)
                .Select(group => new CategoryTotal
                {
                    Category = group.Key,
                    TotalValue = group.Sum(p => p.Price)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            // Simulate database connection delay
            await Task.Delay(1000);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Enumerable.Empty<Product>();
            }

            return await _context.Products
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }
        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            var existingProduct = await _context.Products.FindAsync(updatedProduct.Id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Category = updatedProduct.Category;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
