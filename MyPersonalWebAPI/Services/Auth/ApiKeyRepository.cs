using Microsoft.EntityFrameworkCore;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services;

namespace MyPersonalWebAPI.Auth
{
    public class ApiKeyRepository: ServiceBase<ApiKey>,IApiKeyRepository
    {
        private readonly ILogger<ApiKeyRepository> _logger;

        public ApiKeyRepository(ILogger<ApiKeyRepository> logger, 
                                DatabaseContext context ): base(context)
        {
            _logger = logger;
        }

        public async Task<ApiKey> GetByKey(string key)
        {
            var apiKey = await _context.apiKey.FirstOrDefaultAsync( x => x.Key == key );
            return apiKey;
        }
    }
}