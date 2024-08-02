using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InForno.Interfaces;
using InForno.Models;
using InForno.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;

namespace InForno.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagerProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IIngredientService _ingredientService;

        public ManagerProductsController(IProductService productService, IIngredientService ingredientService)
        {
            _productService = productService;
            _ingredientService = ingredientService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        public async Task<IActionResult> Create()
        {
            ViewBag.Ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto, List<int> selectedIngredients)
        {
            if (productDto.Photo == null || productDto.Photo.Length == 0)
            {
                ModelState.AddModelError("Photo", "Product image is required");
                ViewBag.Ingredients = await _ingredientService.GetAllIngredientsAsync();
                return View(productDto);
            }

            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (productDto.Photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productDto.Photo.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }

                await _productService.AddProductAsync(productDto, imageBytes, selectedIngredients);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View(productDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                DeliveryTime = product.DeliveryTimeInMinutes,
                Ingredients = string.Join(", ", product.Ingredients.Select(i => i.Name))
            };

            ViewBag.Ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto productDto, List<int> selectedIngredients)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (productDto.Photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productDto.Photo.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }

                await _productService.UpdateProductAsync(id, productDto, imageBytes, selectedIngredients);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View(productDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
