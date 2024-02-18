using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Roles> Roles => Set<Roles>();
    }
}
