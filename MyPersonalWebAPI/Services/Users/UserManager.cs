using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.JWT;
using MyPersonalWebAPI.Services.Roles;
using Microsoft.AspNetCore.Identity;

namespace MyPersonalWebAPI.Services.Users
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly IUserServices _userServices;
        private readonly IRolesServices _rolesServices;
        private readonly IJWTServices _JWTServices;
        public UserManager(ILogger<UserManager> logger,
                           IUserServices userServices,
                           IRolesServices rolesServices,
                           IJWTServices JWTServices)
        {
            _logger = logger;
            _userServices = userServices;
            _rolesServices = rolesServices;
            _JWTServices = JWTServices;
        }

        public async Task<User> CreateUser(User newUser)
        {
            try
            {
                newUser.CreatedDate = DateTime.UtcNow;
                newUser.UserId = Guid.NewGuid();
                newUser.LastModifiedDate = newUser.CreatedDate;
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                await _userServices.Add(newUser);
                newUser.Password = "";

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error to created user '{newUser.Username}'.", ex);
                throw new Exception($"Error to created user '{newUser.Username}'.", ex);
            }

        }


        public async Task<string> UserAuthenticate(string username, string password)
        {
            try
            {
                var user = await _userServices.GetByName(username);

                if (user == null)
                    throw new UnauthorizedAccessException($"user: {username} not found");


                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                    throw new UnauthorizedAccessException("Incorrect password");

                var tokenString = _JWTServices.GenerateToken(user);

                return tokenString;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error to authenticate user'{username}'", ex);
                throw new Exception($"Error to authenticate user'{username}'", ex);
            }

        }

    }
}