using Microsoft.Extensions.Options;
using MyPersonalWebAPI.Models;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace MyPersonalWebAPI.Services.OpenIA.ChatGPT
{
    public class ChatGPTServices : IChatGPTServices
    {
        private readonly ILogger<ChatGPTServices> _logger;
        private readonly OpenAIAPI openAIAPI;
        private readonly IOptions<SecretsOptions> _options;
        private readonly List<Conversation> conversations;


        public ChatGPTServices(ILogger<ChatGPTServices> logger,
                               IOptions<SecretsOptions> options)
        {
            _logger = logger;
            _options = options;
            openAIAPI = new OpenAIAPI(_options.Value.ApiKeyOpenIA);
        }

        public async Task<string> Execute(string textUser, string user)
        {
            try
            {

                var chat = conversations.FirstOrDefault(x => x.RequestParameters.user == user);
                if (chat == null)
                {
                    chat = openAIAPI.Chat.CreateConversation();
                    chat.Model = Model.ChatGPTTurbo;
                    chat.RequestParameters.Temperature = 0;
                    chat.RequestParameters.user = user;

                    conversations.Add(chat);
                }

                chat.AppendUserInput(textUser);
                return await chat.GetResponseFromChatbotAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return "Lo siento, sucedio un problema, intentalo mas tarde";
            }
        }
    }
}
