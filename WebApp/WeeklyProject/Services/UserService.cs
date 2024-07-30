using WeeklyProject.Interfaces;
using WeeklyProject.Models.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WeeklyProject.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserDto GetUser(UserDto userDto)
        {
            UserDto foundUser = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("SqlServer")))
            {
                var query = @"
                    SELECT u.Id, u.Name, u.Email, r.Name AS RoleName
                    FROM Users u
                    JOIN RoleUser ru ON u.Id = ru.UsersId
                    JOIN Roles r ON ru.RolesId = r.Id
                    WHERE u.Email = @Email AND u.Password = @Password";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", userDto.Email);
                    command.Parameters.AddWithValue("@Password", userDto.Password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foundUser = new UserDto
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Role = reader.GetString(3)
                            };
                        }
                    }
                    connection.Close();
                }
            }

            return foundUser;
        }

        public void Login(UserDto user)
        {
            var context = _httpContextAccessor.HttpContext;
            context.Session.SetInt32("UserId", user.Id);
            context.Session.SetString("UserRole", user.Role);
        }

        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            context.Session.Clear();
        }
    }
}
