using WeeklyProject.Models.Dto;

namespace WeeklyProject.Interfaces
{
    public interface IUserService
    {
        UserDto GetUser(UserDto userDto);
        void Login(UserDto user);
        void Logout();
    }
}
