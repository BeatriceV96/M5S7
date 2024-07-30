using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WeeklyProject.Models;
using WeeklyProject.Context;
using Microsoft.EntityFrameworkCore;

namespace WeeklyProject.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Summary", new { id = order.Id });
            }
            return View(order);
        }

        [HttpGet]
        public IActionResult Summary(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.Id == id);
            return View(order);
        }
    }
}
