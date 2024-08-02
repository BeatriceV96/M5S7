using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InForno.Interfaces;
using InForno.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using InForno.Service;
using InForno.Dto;

namespace InForno.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IProductService productService, IOrderService orderService)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = GetUserId();
            await _cartService.AddToCartAsync(userId, productId, quantity);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            await _cartService.RemoveFromCartAsync(userId, productId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(int productId, int quantity)
        {
            var userId = GetUserId();
            await _cartService.UpdateCartItemAsync(userId, productId, quantity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string shippingAddress, string notes)
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartItemsAsync(userId);

            if (!cartItems.Any())
            {
                ModelState.AddModelError(string.Empty, "Your cart is empty");
                return View(cartItems);
            }

            var orderDto = new OrderDto
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = shippingAddress,
                Notes = notes,
                IsProcessed = false,
                OrderDetails = cartItems.Select(ci => new OrderDetailDto
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            await _orderService.CreateOrderAsync(orderDto);
            await _cartService.ClearCartAsync(userId);

            return RedirectToAction("OrderConfirmation", new { orderId = orderDto.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return View(order);
        }

        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }
    }
}
