using InForno.Context;
using InForno.Dto;
using InForno.Interfaces;
using InForno.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InForno.Service
{
    public class ProductService : IProductService
    {
        private readonly InFornoDbContext _context;

        public ProductService(InFornoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Ingredients)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddProductAsync(ProductDto productDto, byte[] imageBytes, List<int> selectedIngredients)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Photo = imageBytes,
                Price = productDto.Price,
                DeliveryTimeInMinutes = productDto.DeliveryTime,
                Ingredients = new List<Ingredient>()
            };

            if (selectedIngredients != null && selectedIngredients.Any())
            {
                foreach (var ingredientId in selectedIngredients)
                {
                    var ingredient = await _context.Ingredients.FindAsync(ingredientId);
                    if (ingredient != null)
                    {
                        product.Ingredients.Add(ingredient);
                    }
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductDto productDto, byte[] imageBytes, List<int> selectedIngredients)
        {
            var product = await _context.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null;
            }

            product.Name = productDto.Name;
            if (imageBytes != null)
            {
                product.Photo = imageBytes;
            }
            product.Price = productDto.Price;
            product.DeliveryTimeInMinutes = productDto.DeliveryTime;
            product.Ingredients.Clear();
            if (selectedIngredients != null && selectedIngredients.Any())
            {
                foreach (var ingredientId in selectedIngredients)
                {
                    var ingredient = await _context.Ingredients.FindAsync(ingredientId);
                    if (ingredient != null)
                    {
                        product.Ingredients.Add(ingredient);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
