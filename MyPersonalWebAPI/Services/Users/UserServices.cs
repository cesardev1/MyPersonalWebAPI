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

        public async Task<User> CreateUser(User newUser)
        {
            try
            {
                newUser.CreatedDate = DateTime.UtcNow;
                newUser.UserId = Guid.NewGuid();
                newUser.LastModifiedDate = newUser.CreatedDate;
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                _context.Users.Add(newUser);

                await _context.SaveChangesAsync();
                newUser.Password="";

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return null;
            }
            
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

        public async Task<User> GetByPhone(string phone)
        {
            try
            {
                return await base._context.Users.Include(a => a.Role ).FirstOrDefaultAsync(x=> x.Phone.Equals(phone));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message,ex.StackTrace);
                return null;
            }
        }

        public async  Task<User> UserAuthenticate(string username, string password)
        {
            var user = await GetByName(username);

            if (user !=null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;
            return null;
        }
    }
}
