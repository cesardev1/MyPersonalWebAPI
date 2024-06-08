using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Request;

namespace MyPersonalWebAPI.Services.Users
{
    public interface IUserManager
    {
        Task<User> CreateUser(NewUserRequest newUser);
        Task<string> UserAuthenticate(string username, string password);
        Task<List<User>> GetAllUser();
    }
}