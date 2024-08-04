using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(registerDto);
                if (result)
                {
                    var loginDto = new LoginDto { Username = registerDto.Username, Password = registerDto.Password };
                    var user = await _userService.Login(loginDto);
                    if (user != null)
                    {
                        return RedirectToAction("Products", "Cart");
                    }
                }
                ModelState.AddModelError("", "Registration failed");
            }
            return View(registerDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.Login(loginDto);
                if (user == null)
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid credentials");
                    return View(loginDto);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(loginDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
