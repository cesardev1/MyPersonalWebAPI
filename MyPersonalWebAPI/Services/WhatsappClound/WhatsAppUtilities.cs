using Microsoft.Extensions.Options;
using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Services.WhatsappClound
{
    public class WhatsAppUtilities
    {
        private readonly IOptions<SecretsOptions> _options;
        private readonly ILogger<WhatsAppUtilities> _logger;

        public WhatsAppUtilities(IOptions<SecretsOptions> options,
                                 ILogger<WhatsAppUtilities> logger)
        {
            _options = options;
            _logger = logger;
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

        public bool ValidationTokenUrlWhatsapp(string token)
        {
            bool result = _options.Value.AccessTokenWhatsapp.Equals(token);
            return result;
        }

    }
}