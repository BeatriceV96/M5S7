using Microsoft.AspNetCore.Mvc;
using WeeklyProject.Models.Dto;
using WeeklyProject.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserDto model)
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

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,Email,Password")] UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("SqlServer")))
            {
                var query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password); SELECT SCOPE_IDENTITY();";
                int userId;
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    connection.Open();
                    userId = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }

                var roleQuery = "INSERT INTO RoleUser (RolesId, UsersId) VALUES ((SELECT Id FROM Roles WHERE Name = 'User'), @UserId)";
                using (var roleCommand = new SqlCommand(roleQuery, connection))
                {
                    roleCommand.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    roleCommand.ExecuteNonQuery();
                    connection.Close();
                }

                model.Id = userId;
                model.Role = "User";
            }

            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
