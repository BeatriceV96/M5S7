using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InForno.Interfaces;
using InForno.Models;
using InForno.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace WeeklyProject.Controllers
{
    [Authorize(Roles = "Admin,Manager")] // Limita l'accesso solo agli amministratori e ai gestori
    public class ManagerProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ManagerProductsController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
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
        public async Task<IActionResult> Create(ProductDto productDto, List<int> selectedIngredients)
        {
            if (productDto.Photo == null || productDto.Photo.Length == 0)
            {
                ModelState.AddModelError("Photo", "Product image is required");
                ViewBag.product = await _productService.GetAllProductsAsync();
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

            ViewBag.Ingredients = await _productService.GetAllProductsAsync();
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

            ViewBag.ProductId = id;
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

        // Metodi per gestire gli ordini
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrderAsync(orderDto);
                return RedirectToAction(nameof(Orders));
            }
            return View(orderDto);
        }

        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto
            {
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                IsProcessed = order.IsProcessed,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            ViewBag.OrderId = id;
            return View(orderDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(int id, OrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(id, orderDto);
                return RedirectToAction(nameof(Orders));
            }
            ViewBag.OrderId = id;
            return View(orderDto);
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Orders));
        }
    }
}
