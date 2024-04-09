using MyPersonalWebAPI.Services;

namespace MyPersonalWebAPI.Auth
{
    public interface IApiKeyRepository: IServiceBase<ApiKey>
    {
        Task<ApiKey> GetByKey(string key);
    }
}