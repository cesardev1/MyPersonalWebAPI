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

    }
}
