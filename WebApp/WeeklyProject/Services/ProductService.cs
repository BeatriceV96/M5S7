using Microsoft.EntityFrameworkCore;
using WeeklyProject.Context;
using WeeklyProject.Interfaces;
using WeeklyProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeklyProject.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dbContext;

        public ProductService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products
                .Include(p => p.Ingredients)
                .ToListAsync();
        }
    }
}
