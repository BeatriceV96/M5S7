using Microsoft.AspNetCore.Mvc;
using WeeklyProject.Models.Dto;
using WeeklyProject.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WeeklyProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Email,Password")] UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.GetUser(model);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email o password errati");
                return View(model);
            }

            _userService.Login(user);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _userService.Logout();
            return RedirectToAction("Index");
        }
    }
}
