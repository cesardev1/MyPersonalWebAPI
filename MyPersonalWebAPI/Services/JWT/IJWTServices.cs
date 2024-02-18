using MyPersonalWebAPI.Models;
using System.Security.Claims;

namespace MyPersonalWebAPI.Services.JWT
{
    public interface IJWTServices
    {
        public string GenerateToken(string username);
        public bool VerifyToken(string token);
    }
}
