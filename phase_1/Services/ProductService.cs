using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.Models;
using phase_1.Services.Interfaces;

namespace phase_1.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            // Init mock data with 10-15 products
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "iPhone 15 Pro", Price = 1000m, Category = "Smartphone" },
                new Product { Id = 2, Name = "Samsung Galaxy S24 Ultra", Price = 1200m, Category = "Smartphone" },
                new Product { Id = 3, Name = "Google Pixel 8 Pro", Price = 900m, Category = "Smartphone" },
                new Product { Id = 4, Name = "Xiaomi 14 Pro", Price = 850m, Category = "Smartphone" },
                new Product { Id = 5, Name = "OnePlus 12", Price = 800m, Category = "Smartphone" },

                new Product { Id = 6, Name = "MacBook Pro 16", Price = 2500m, Category = "Laptop" },
                new Product { Id = 7, Name = "Dell XPS 15", Price = 2000m, Category = "Laptop" },
                new Product { Id = 8, Name = "ThinkPad X1 Carbon", Price = 1800m, Category = "Laptop" },
                new Product { Id = 9, Name = "Asus ROG Zephyrus", Price = 2200m, Category = "Laptop" },
                new Product { Id = 10, Name = "HP Spectre x360", Price = 1500m, Category = "Laptop" },

                new Product { Id = 11, Name = "Sony WH-1000XM5", Price = 350m, Category = "Audio" },
                new Product { Id = 12, Name = "AirPods Pro 2", Price = 250m, Category = "Audio" },
                new Product { Id = 13, Name = "Bose QuietComfort Earbuds II", Price = 300m, Category = "Audio" }
            };
        }

        public IEnumerable<Product> GetTop3HighestPricedProductsByCategory(string category)
        {
            return _products
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(p => p.Price)
                .Take(3)
                .ToList();
        }

        public IEnumerable<CategoryTotal> GetTotalPriceByCategory()
        {
            return _products
                .GroupBy(p => p.Category)
                .Select(group => new CategoryTotal
                {
                    Category = group.Key,
                    TotalValue = group.Sum(p => p.Price)
                })
                .ToList();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            // Simulate database connection delay
            await Task.Delay(1000);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Enumerable.Empty<Product>();
            }

            return _products
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
