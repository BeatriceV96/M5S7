using InForno.Dto;
using InForno.Models;

namespace InForno.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<User> Login(LoginDto logindto);
        Task Logout();
    }
}
