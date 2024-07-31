using Microsoft.EntityFrameworkCore;
using WeeklyProject.Context;
using WeeklyProject.Interface;
using WeeklyProject.Models;

namespace WeeklyProject.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _dbContext;

        public IngredientService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            _dbContext.Ingredients.Add(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            _dbContext.Ingredients.Update(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteIngredientAsync(int ingredientId)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(ingredientId);
            if (ingredient != null)
            {
                _dbContext.Ingredients.Remove(ingredient);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int ingredientId)
        {
            return await _dbContext.Ingredients.FindAsync(ingredientId);
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _dbContext.Ingredients.ToListAsync();
        }
    }
}
