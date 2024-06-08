using Microsoft.EntityFrameworkCore;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.Roles
{
    public class RolesServices : ServiceBase<Models.Roles>, IRolesServices
    {
        private readonly ILogger<RolesServices> _logger;

        public RolesServices(ILogger<RolesServices> logger,
                             DatabaseContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task<Models.Roles> GetRolesById(int id)
        {
            try
            {
                var role = await base._context.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
                return role;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return null;
            }

        }

        public async Task<Models.UserRole> AddRoleUser(Guid userId, int roleId)
        {

            var newRoleUser = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };
            var ResponseAddRoleUser = await base._context.userRoles.AddAsync(newRoleUser);
            await base._context.SaveChangesAsync();

            return ResponseAddRoleUser.Entity;
        }

        public async Task<List<Models.Roles>> GetUserRoles(Guid userId)
        {
            List<Models.Roles> roles = new List<Models.Roles>();

            var RoleList = await base._context.userRoles
                                .Where(x => x.UserId == userId)
                                .Include(r => r.Role)
                                .ToListAsync();

            foreach (var item in RoleList)
            {

                roles.Add(item.Role);
            }
            return roles;
        }


    }
}
