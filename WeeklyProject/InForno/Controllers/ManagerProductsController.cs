using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InForno.Interfaces;
using InForno.Models;
using InForno.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;

namespace WeeklyProject.Controllers
{
    [Authorize(Roles = "Admin,Manager")] // Limita l'accesso solo agli amministratori e ai gestori
    public class ManagerProductsController : Controller
    {
        private readonly IProductService _productService;

        public ManagerProductsController(IProductService productService)
        {
            _productService = productService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto, IFormFile photo, List<int> selectedIngredients)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }

                await _productService.AddProductAsync(productDto, imageBytes, selectedIngredients);
                return RedirectToAction(nameof(Index));
            }
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
                Name = product.Name,
                Photo = product.Photo,
                Price = product.Price,
                DeliveryTime = product.DeliveryTimeInMinutes,
                Ingredients = string.Join(", ", product.Ingredients.Select(i => i.Name))
            };

            ViewBag.ProductId = id;
            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto productDto, IFormFile photo, List<int> selectedIngredients)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }

                await _productService.UpdateProductAsync(id, productDto, imageBytes, selectedIngredients);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProductId = id; 
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
