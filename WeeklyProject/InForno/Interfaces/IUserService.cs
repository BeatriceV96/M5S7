using InForno.Dto;
using InForno.Models;

namespace InForno.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterDto register);
        Task<User> Login(LoginDto logindto);
        Task Logout();
    }
}
