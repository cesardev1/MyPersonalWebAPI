using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;

namespace MyPersonalWebAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Roles> Roles => Set<Roles>();
        public DbSet<WhatsAppMessage> whatsAppMessages => Set<WhatsAppMessage>();
    }
}
