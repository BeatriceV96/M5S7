using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using InForno.Interfaces;
using InForno.Dto;

namespace InForno.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (RegisterDto register)
        {
            if (ModelState.IsValid)
            {
                await _userService.Register(register);
                return RedirectToAction("Login");
            }
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.Login(logindto);
                if (user == null)
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid credentials");
                    return View(logindto);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(logindto);
        }
        
        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

    }
}
