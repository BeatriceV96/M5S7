﻿using InForno.Dto;
using InForno.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InForno.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductDto productDto, byte[] imageBytes, List<int> selectedIngredients);
        Task<Product> UpdateProductAsync(int id, ProductDto productDto, byte[] imageBytes, List<int> selectedIngredients);
        Task<bool> DeleteProductAsync(int id);
    }
}