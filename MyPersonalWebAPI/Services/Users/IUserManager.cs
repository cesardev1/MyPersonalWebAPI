using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Users
{
    public interface IUserManager
    {
        Task<User> CreateUser(User newUser);
        Task<string> UserAuthenticate(string username, string password);
        Task<List<User>> GetAllUser();
    }
}