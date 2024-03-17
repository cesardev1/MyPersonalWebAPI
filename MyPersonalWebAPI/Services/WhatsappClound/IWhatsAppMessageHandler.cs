using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;

namespace MyPersonalWebAPI.Services.WhatsappClound
{
    public interface IWhatsAppMessageHandler
    {
        Task<bool> Sample(string phone);
        bool UrlTokenValidator(string token, string challenge);
        Task<string> AutoResponse(WhatsAppCloudModel message);
    }
}