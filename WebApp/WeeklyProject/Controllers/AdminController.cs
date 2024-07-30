using Microsoft.AspNetCore.Mvc;
using WeeklyProject.Models;
using WeeklyProject.Context;
using Microsoft.EntityFrameworkCore;

namespace WeeklyProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin())
                return Unauthorized();

            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAdmin())
                return Unauthorized();

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!IsAdmin())
                return Unauthorized();

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Orders()
        {
            if (!IsAdmin())
                return Unauthorized();

            var orders = _context.Orders.Include(o => o.User).ToList();
            return View(orders);
        }

        [HttpPost]
        public IActionResult MarkOrderAsProcessed(int id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var order = _context.Orders.Find(id);
            if (order != null)
            {
                order.IsProcessed = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Orders");
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
    }
}
