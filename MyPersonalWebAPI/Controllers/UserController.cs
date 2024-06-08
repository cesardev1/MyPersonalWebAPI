using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyPersonalWebAPI.Auth;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Request;
using MyPersonalWebAPI.Models.Response;
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
        public async Task<IActionResult> CreateUser([FromBody] NewUserRequest user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                var newUser = await _userManager.CreateUser(user);
                return Ok(new SuccessResponse<User>
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = newUser
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "400",
                    ErrorMessage = ex.Message,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error",
                    Timestamp = DateTime.Now
                });
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

                return Ok(new SuccessResponse<string>
                {
                    Success = true,
                    Message = "User authenticated successfully",
                    Data = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "401",
                    ErrorMessage = ex.Message,
                    Timestamp = DateTime.Now
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error",
                    Timestamp = DateTime.Now
                });
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
                return Ok(new SuccessResponse<ApiKey>
                {
                    Success = true,
                    Message = "Apikey created successfully",
                    Data = key
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "400",
                    ErrorMessage = ex.Message,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error",
                    Timestamp = DateTime.Now
                });
            }
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userManager.GetAllUser();
                return Ok(new SuccessResponse<IEnumerable<User>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error",
                    Timestamp = DateTime.Now
                });
            }
        }
    }
}
