using Microsoft.Extensions.Options;
using System.Text;
using MyPersonalWebAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using MyPersonalWebAPI.Models.Whatsapp;
using MyPersonalWebAPI.Services.Users;
using MyPersonalWebAPI.Data;

namespace MyPersonalWebAPI.Services.WhatsappClound.SendMessage
{
    public class WhatsappCloudSendMessageServices:IWhatsappCloudSendMessageServices
    {
        private readonly IOptions<SecretsOptions> _options;
        private readonly ILogger<WhatsappCloudSendMessageServices> _logger;
        private readonly UserServices _userServices;
        private readonly DatabaseContext _context;
        public WhatsappCloudSendMessageServices(IOptions<SecretsOptions> options,
                                                ILogger<WhatsappCloudSendMessageServices> logger,
                                                UserServices userServices,
                                                DatabaseContext context)
        {
            _options = options;
            _logger = logger;
            _userServices = userServices;
            _context = context;
        }
        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            Console.Write(byteData);

            using (var content = new ByteArrayContent(byteData))
            {
                //TODO move to enviroment variables
                string endpoint = "https://graph.facebook.com/";
                string phoneNumberId = _options.Value.WhatsappPhoneId;
                string whatsappApikey = _options.Value.ApiKeyWhatsapp;
                string uri = $"{endpoint}/v16.0/{phoneNumberId}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {whatsappApikey}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public string GetUserText(Message message)
        {
            string typeMessage = message.Type.ToUpper();


            if (typeMessage == "TEXT")
                return message.Text.Body;

            else if (typeMessage == "INTERACTIVE")
            {
                string interactiveType = message.Interactive.Type.ToUpper();

                if (interactiveType == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                }
                else if (interactiveType == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<bool> SaveMessage(Message message, MessageDirection direction)
        {
            try
            {
                var user =await _userServices.GetByPhone(message.From);

                var messageDb = new WhatsAppMessage();
                messageDb.Direction = direction;
                messageDb.MessageText = message.Text.Body;
                messageDb.UserId = user.UserId;
                messageDb.MessageId = message.Id;
                messageDb.Timestamp = DateTime.UtcNow;

                await _context.whatsAppMessages.AddAsync(messageDb);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex,ex.StackTrace);
                return false;
            }
        }

        public bool ValidationTokenUrlWhatsapp(string token)
        {
            bool result = _options.Value.AccessTokenWhatsapp.Equals(token);
            return result;
        }
    }
}
