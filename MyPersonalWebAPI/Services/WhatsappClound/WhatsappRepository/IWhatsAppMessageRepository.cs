using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;

namespace MyPersonalWebAPI.Services.WhatsappClound
{
    public interface IWhatsAppMessageRepository : IServiceBase<WhatsAppMessage>
    {
        Task<bool> SaveMessage(WhatsAppMessage message);
    }
}