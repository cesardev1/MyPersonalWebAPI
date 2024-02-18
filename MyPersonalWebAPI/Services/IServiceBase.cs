namespace MyPersonalWebAPI.Services
{
    public interface IServiceBase<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(string id);
    }
}
