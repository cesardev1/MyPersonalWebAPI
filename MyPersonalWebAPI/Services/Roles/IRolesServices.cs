using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Roles
{
    public interface IRolesServices : IServiceBase<Models.Roles>
    {
        Task<Models.Roles> GetRolesById(int id);
    }
}
