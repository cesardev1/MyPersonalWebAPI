using MyPersonalWebAPI.Models;
using System.Security.Claims;

namespace MyPersonalWebAPI.Services.JWT
{
    public interface IJWTServices
    {
        public string GenerateToken(User user);
        public bool VerifyToken(string token);
    }
}
