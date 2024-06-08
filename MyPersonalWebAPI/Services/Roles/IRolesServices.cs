using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Roles
{
    public interface IRolesServices : IServiceBase<Models.Roles>
    {
        Task<Models.Roles> GetRolesById(int id);
        Task<Models.UserRole> AddRoleUser(Guid userId, int RoleId);

        Task<List<Models.Roles>> GetUserRoles(Guid userId);
    }
}
