using WeeklyProject.Models;

namespace WeeklyProject.Interface
{
    public interface IIngredientService
    {
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int ingredientId);
        Task<Ingredient?> GetIngredientByIdAsync(int ingredientId);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
    }
}
