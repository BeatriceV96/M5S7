using Microsoft.AspNetCore.Mvc;
using WeeklyProject.Models.Dto;
using WeeklyProject.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WeeklyProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
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

            _userService.Login(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Password")] UserDto model)
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

                _userService.Login(model);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}

