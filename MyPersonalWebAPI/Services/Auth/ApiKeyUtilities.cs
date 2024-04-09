using System.Security.Cryptography;

namespace MyPersonalWebAPI.Auth
{
    public class ApiKeyUtilities
    {
        private readonly ILogger<ApiKeyUtilities> _logger;
        public ApiKeyUtilities(ILogger<ApiKeyUtilities> logger)
        {
            _logger = logger;
        }

        public string GenerateApiKey()
        {
            try
            {
                const int keyLength = 32; // Longitud deseada de la clave en bytes
                byte[] buffer = new byte[keyLength];
    
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(buffer);
                }
    
                // Codificar los bytes en una cadena de caracteres Base64
                string apiKey = Convert.ToBase64String(buffer)
                    .Replace("+", "-") // Reemplazar '+'  por '-' (URL-safe)
                    .Replace("/", "_") // Reemplazar '/' por '_' (URL-safe)
                    .TrimEnd('='); // Eliminar el '=' al final
    
                return apiKey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating API key");
                throw;
            }
        }
    }
}