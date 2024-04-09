using Microsoft.EntityFrameworkCore.Storage;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;
using MyPersonalWebAPI.Services;

namespace MyPersonalWebAPI.Services.WhatsappClound
{
    class WhatsAppMessageRepository : ServiceBase<WhatsAppMessage>, IWhatsAppMessageRepository
    {
        private readonly ILogger<WhatsAppMessageRepository> _logger;
        public WhatsAppMessageRepository(DatabaseContext context,
                                         ILogger<WhatsAppMessageRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SaveMessage(WhatsAppMessage message)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.whatsAppMessages.AddAsync(message);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.StackTrace);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}