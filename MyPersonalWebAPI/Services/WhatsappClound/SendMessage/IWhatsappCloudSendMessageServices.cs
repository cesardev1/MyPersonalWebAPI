using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;

namespace MyPersonalWebAPI.Services.WhatsappClound.SendMessage
{
    public interface IWhatsappCloudSendMessageServices
    {
        Task<bool> Execute(object model);
    }
}
