using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Users
{
    public interface IUserServices: IServiceBase<User>
    {
        Task<User> CreateUser(User newUser);
        Task<User> GetByName(string name);
        new Task<User> GetById(string id);
        Task<User> GetByPhone(string phone);
        Task<User> UserAuthenticate(string username, string password);
    }
}
