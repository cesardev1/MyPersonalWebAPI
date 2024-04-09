
using MyPersonalWebAPI.Auth;

namespace MyPersonalWebAPI.Services.Auth
{
    public interface IApiKeyManager
    {
        Task<ApiKey> CreateApiKeyAsync(string name, Guid userId);
        Task<ApiKey> ValidateApiKeyAsync(string apiKey);
    }
}