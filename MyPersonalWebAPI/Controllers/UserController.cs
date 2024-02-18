using Microsoft.AspNetCore.Mvc;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.JWT;
using MyPersonalWebAPI.Services.Users;

namespace MyPersonalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<UserController> _logger;
        private readonly IJWTServices _JWTServices;
        public UserController(IUserServices userServices,
                              ILogger<UserController> logger,
                              IJWTServices JWTServices)
        {
            _userServices = userServices;
            _logger = logger;
            _JWTServices = JWTServices;
        }

        [HttpPost]
        public dynamic CreateUser([FromBody] User user)
        {
            try
            {
                user.CreatedDate = DateTime.UtcNow;
                user.LastModifiedDate = user.CreatedDate;
                user.UserId = Guid.NewGuid();

                _userServices.Add(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
