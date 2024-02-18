using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.Roles;
using Microsoft.EntityFrameworkCore;
using MyPersonalWebAPI.Data;

namespace MyPersonalWebAPI.Services.Users
{
    public class UserServices : ServiceBase<User>, IUserServices
    {
        private readonly ILogger<UserServices> _logger;
        private readonly IRolesServices _rolesServices;
        
        public UserServices(DatabaseContext context,
                            ILogger<UserServices> logger,
                            IRolesServices rolesServices): base(context)
        {
            _logger = logger;
            _rolesServices = rolesServices;
        }

        public async override Task<User> GetById(string id)
        {
            using (var context = base._context.Database.BeginTransaction())
            {
                try
                {
                    var idGuid = new Guid(id);
                    return await base._context.Users.Include(a=>a.Role).FirstOrDefaultAsync(x => x.UserId ==idGuid);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex,ex.StackTrace);
                    return null;
                }
            }
        }

        public async Task<User> GetByName(string name)
        {
            using (var context = base._context.Database.BeginTransaction())
            {
                try
                {
                    return await base._context.Users.Include(a => a.Role).FirstOrDefaultAsync(x => x.Username == name);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.StackTrace);
                    return null;
                }
            }
        }

        
        public async  Task<User> UserAuthenticate(string username, string password)
        {
            var user = await GetByName(username);

            if (user !=null && user.Password == password)
                return user;

            return null;
        }
    }
}
