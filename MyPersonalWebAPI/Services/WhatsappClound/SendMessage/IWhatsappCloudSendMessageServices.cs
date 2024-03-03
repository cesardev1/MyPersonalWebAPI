using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;

namespace MyPersonalWebAPI.Services.WhatsappClound.SendMessage
{
    public interface IWhatsappCloudSendMessageServices
    {
        Task<bool> Execute(object model);

        bool ValidationTokenUrlWhatsapp(string token);

        string GetUserText(Message message);
        Task<bool> SaveMessage(Message message, MessageDirection direction);
    }
}
