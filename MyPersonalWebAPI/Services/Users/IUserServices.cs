using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Users
{
    public interface IUserServices: IServiceBase<User>
    {
        Task<User> GetByName(string name);
        new Task<User> GetById(string id);
        Task<User> GetByPhone(string phone);
    }
}
