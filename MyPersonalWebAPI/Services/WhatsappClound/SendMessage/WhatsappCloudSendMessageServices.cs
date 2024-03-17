using Microsoft.Extensions.Options;
using System.Text;
using MyPersonalWebAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using MyPersonalWebAPI.Models.Whatsapp;
using MyPersonalWebAPI.Services.Users;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Services.WhatsappClound;

namespace MyPersonalWebAPI.Services.WhatsappClound.SendMessage
{
    public class WhatsappCloudSendMessageServices:IWhatsappCloudSendMessageServices
    {
        private readonly IOptions<SecretsOptions> _options;
        private readonly ILogger<WhatsappCloudSendMessageServices> _logger;
        private readonly DatabaseContext _context;
        public WhatsappCloudSendMessageServices(IOptions<SecretsOptions> options,
                                                ILogger<WhatsappCloudSendMessageServices> logger,
                                                IUserServices userServices,
                                                DatabaseContext context)
        {
            _options = options;
            _logger = logger;
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


        


    }
}
