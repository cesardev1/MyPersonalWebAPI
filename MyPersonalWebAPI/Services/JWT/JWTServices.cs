﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyPersonalWebAPI.Services.JWT
{
    public class JWTServices: IJWTServices
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<JWTServices> _logger;
        private readonly IOptions<SecretsOptions> _secretsOptions;

        public JWTServices(IUserServices userServices,
                            ILogger<JWTServices> logger,
                            IOptions<SecretsOptions> secretsOptions)
        {
            _userServices = userServices;
            _secretsOptions = secretsOptions;
            _logger = logger;
        }

        public string GenerateToken(string username)
        {
            var symmetricKey = Encoding.ASCII.GetBytes(_secretsOptions.Value.JWTSecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool VerifyToken(string token)
        {
            var symmetricKey = Encoding.ASCII.GetBytes(_secretsOptions.Value.JWTSecretKey);
            var tokenHandler = new  JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validateToken);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
