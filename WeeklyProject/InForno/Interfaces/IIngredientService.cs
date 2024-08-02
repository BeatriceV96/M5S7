using InForno.Dto;
using InForno.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InForno.Interfaces
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<Ingredient> AddIngredientAsync(IngredientDto ingredientDto);
        Task<Ingredient> UpdateIngredientAsync(int id, IngredientDto ingredientDto);
        Task<bool> DeleteIngredientAsync(int id);
    }
}
