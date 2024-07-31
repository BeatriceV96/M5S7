using WeeklyProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeklyProject.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
