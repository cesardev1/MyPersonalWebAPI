using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyPersonalWebAPI.Auth;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Request;
using MyPersonalWebAPI.Services.Auth;
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
        private readonly IApiKeyManager _apikeyManager;
        public UserController(IUserManager userManager,
                              ILogger<UserController> logger,
                              IJWTServices JWTServices,
                              IApiKeyManager apikeyManager)
        {
            _userManager = userManager;
            _logger = logger;
            _apikeyManager = apikeyManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                await _userManager.CreateUser(user);
                return Ok();
            }
            catch (ArgumentException ex)
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
        public async Task<IActionResult> LogIn([FromBody] LogInRequest userData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var token = await _userManager.UserAuthenticate(userData.Username, userData.Password);

                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // creat endopoint to add new apikey to user
        [Authorize]
        [HttpPost("apikey")]
        public async Task<IActionResult> AddApiKey([FromBody] ApikeyRequest apiKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                // get user id from token
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var key = await _apikeyManager.CreateApiKeyAsync(apiKey.Name, userId);
                return Ok(key);
            }
            catch (ArgumentException ex)
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

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userManager.GetAllUser();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
