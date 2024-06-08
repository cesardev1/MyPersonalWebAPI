using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.JWT;
using MyPersonalWebAPI.Services.Roles;
using Microsoft.AspNetCore.Identity;
using MyPersonalWebAPI.Models.Request;
using AutoMapper;
using MyPersonalWebAPI.Exceptions;

namespace MyPersonalWebAPI.Services.Users
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;
        private readonly IUserServices _userServices;
        private readonly IRolesServices _rolesServices;
        private readonly IJWTServices _JWTServices;
        private readonly IMapper _mapper;
        public UserManager(ILogger<UserManager> logger,
                           IUserServices userServices,
                           IRolesServices rolesServices,
                           IJWTServices JWTServices,
                           IMapper mapper)
        {
            _logger = logger;
            _userServices = userServices;
            _rolesServices = rolesServices;
            _JWTServices = JWTServices;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(NewUserRequest newUser)
        {
            try
            {


                User user = _mapper.Map<User>(newUser);

                user.UserId = Guid.NewGuid();
                user.CreatedDate = DateTime.UtcNow;
                user.LastModifiedDate = user.CreatedDate;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);


                //TODO: Refactor user's service to return user

                await _userServices.Add(user);


                var createdUser = await _userServices.GetByName(user.Username);

                foreach (var rol in newUser.Roles)
                {
                    //var addrol = await _rolesServices.GetRolesById(rol);

                    // if(addrol==null)
                    //     throw new RoleNotFoundException($"Role with ID '{rol}' not found.");//validar el retorno de error 

                    await _rolesServices.AddRoleUser(createdUser.UserId, rol);
                }
                user.Password = "";

                return user;
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

                user.UserRoles = await _rolesServices.GetUserRoles(user.UserId);

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

        public async Task<List<User>> GetAllUser()
        {
            try
            {
                var listUser = await _userServices.GetAll();
                return listUser.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to get all users");
                throw new Exception("Error to get all users", ex);
            }
        }
    }
}