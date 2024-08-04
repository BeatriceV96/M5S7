using InForno.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InForno.Dto;
using InForno.Models;
using InForno.Interfaces;

namespace InForno.Service
{
    public class UserService : IUserService
    {
        private readonly InFornoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(InFornoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Register(RegisterDto register)
        {
            var user = new User
            {
                Name = register.Username,
                Password = register.Password,
                Email = register.Email,
                Role = register.Role
            };
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<User> Login(LoginDto logindto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == logindto.Username && x.Password == logindto.Password);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var AuthProperties = new AuthenticationProperties();
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), AuthProperties);
            return user;
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
