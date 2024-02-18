using MyPersonalWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MyPersonalWebAPI.Services
{
    public abstract class ServiceBase<T> : IServiceBase<T> where T : class
    {
        protected DatabaseContext _context;
        public ServiceBase(DatabaseContext context)
        {
            _context = context;
        }
        public virtual async Task Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return;
        }

        public virtual async Task Delete(string id)
        {
            var uuid = new Guid(id);
            var entity = _context.Set<T>().Find(uuid);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetById(string id)
        {
            var uuid = new Guid(id);
            return await _context.Set<T>().FindAsync(uuid);
        }

        public virtual async Task Update(T entity)
        {
            _context.Entry(entity).State = (Microsoft.EntityFrameworkCore.EntityState)EntityState.Modified;
            await _context.SaveChangesAsync();
            return;
        }
    }
}
