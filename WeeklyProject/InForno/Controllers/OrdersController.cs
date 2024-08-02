using InForno.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InForno.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool isProcessed)
        {
            await _orderService.UpdateOrderStatusAsync(id, isProcessed);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DailyReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DailyReport(DateTime date)
        {
            var totalOrders = await _orderService.GetTotalOrdersProcessedAsync(date);
            var totalRevenue = await _orderService.GetTotalRevenueAsync(date);
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            return View();
        }
    }
}
