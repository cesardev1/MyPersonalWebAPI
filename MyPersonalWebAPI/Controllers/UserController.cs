using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {

                await _userServices.CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(500, "Internal Server Error");
            }

        }


        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] User userData)
        {
            try
            {
                var user = await _userServices.UserAuthenticate(userData.Username, userData.Password);
                if(user == null)
                {
                    return Unauthorized();
                }

                var tokenString = _JWTServices.GenerateToken(user.Username);
                return Ok(tokenString);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
