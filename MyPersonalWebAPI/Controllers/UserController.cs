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
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserManager userManager,
                              ILogger<UserController> logger,
                              IJWTServices JWTServices)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                await _userManager.CreateUser(user);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

        }


        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] User userData)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var token  = await _userManager.UserAuthenticate(userData.Username, userData.Password);
                                
                return Ok(token);
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex,ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
