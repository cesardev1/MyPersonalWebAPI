using MyPersonalWebAPI.Auth;
using MyPersonalWebAPI.Services.Auth;

namespace MyPersonalWebAPI.Services
{
    public class ApiKeyManager: IApiKeyManager
    {
        private readonly ILogger<ApiKeyManager> _logger;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly ApiKeyUtilities _apiKeyUtilities;
        public ApiKeyManager(
            IApiKeyRepository apiKeyRepository,
            ILogger<ApiKeyManager> logger, 
            ApiKeyUtilities apiKeyUtilities)
        {
            _logger = logger;
            _apiKeyUtilities = apiKeyUtilities;
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task<ApiKey> ValidateApiKeyAsync(string key)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetByKey(key);

                if (apiKey == null && apiKey.CurrentStatus != Status.Active)
                {
                    throw new UnauthorizedAccessException("Key unauthorized");
                }

                apiKey.User.Password = "";

                return apiKey;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt with API key: {Key}", key);
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error validating API key: {Key}", key);
                throw new ApplicationException("Error validating API key", ex);
            }
        }

        public async Task<ApiKey> CreateApiKeyAsync(string name, Guid userId)
        {
            try
            {
                // Generar una nueva clave de API única
                var apiKey = _apiKeyUtilities.GenerateApiKey();

                // Crear una nueva instancia de ApiKey
                var newApiKey = new ApiKey();
                
                newApiKey.ApiKeyId = Guid.NewGuid();
                newApiKey.Key = apiKey;
                newApiKey.Name = name;
                newApiKey.DateAtCreated = DateTime.UtcNow;
                newApiKey.DateAtUpdated = DateTime.UtcNow;
                newApiKey.CurrentStatus = Status.Pending;
                newApiKey.UserId = userId;
                

                // Guardar la nueva clave de API en la base de datos
                await _apiKeyRepository.Add(newApiKey);

                return newApiKey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating API key: {Name}", name);
                throw new ApplicationException("Error creating API key", ex);
            }
        }


    }
}