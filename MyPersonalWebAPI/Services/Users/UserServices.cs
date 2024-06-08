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
                            IRolesServices rolesServices) : base(context)
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
                    return await base._context.Users.Include(a => a.UserRoles).FirstOrDefaultAsync(x => x.UserId == idGuid);
                }
                catch (FormatException ex)
                {
                    _logger.LogError($"The user ID {id} does not have a valid GUID format", ex);
                    throw new ArgumentException($"The user ID {id} does not have a valid GUID format", ex);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError($"Error to get user with ID '{id}'", ex);
                    throw;
                }
            }
        }

        public async Task<User> GetByName(string name)
        {
            try
            {
                return await base._context.Users.Where(x => x.Username == name).FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error getting user '{name}'", ex);
                throw;
            }
        }

        public async Task<User> GetByPhone(string phone)
        {
            try
            {
                return await base._context.Users.Include(a => a.UserRoles).FirstOrDefaultAsync(x => x.Phone.Equals(phone));
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error to get user with '{phone}'", ex);
                throw;
            }
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await base._context.Users.Include(a => a.UserRoles).ToListAsync();
        }

    }
}
