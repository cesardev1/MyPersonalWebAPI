using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.WhatsappClound.SendMessage
{
    public interface IWhatsappCloudSendMessageServices
    {
        Task<bool> Execute(object model);

        bool ValidationTokenUrlWhatsapp(string token);

        string GetUserText(Message message);
    }
}
