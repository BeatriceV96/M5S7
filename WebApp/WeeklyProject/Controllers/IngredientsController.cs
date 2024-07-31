using Microsoft.AspNetCore.Mvc;
using WeeklyProject.Interface;
using WeeklyProject.Models;

namespace WeeklyProject.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        public async Task<IActionResult> Index()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View(ingredients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _ingredientService.AddIngredientAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _ingredientService.UpdateIngredientAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

