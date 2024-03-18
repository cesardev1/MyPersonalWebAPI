using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;
using MyPersonalWebAPI.Util;
using MyPersonalWebAPI.Services.WhatsappClound;
using MyPersonalWebAPI.Models.Whatsapp;
using MyPersonalWebAPI.Services.Users;
using System.Reflection;
using MyPersonalWebAPI.Services.OpenIA.ChatGPT;

namespace MyPersonalWebAPI.Services.WhatsappClound
{
    public class WhatsAppMessageHandler: IWhatsAppMessageHandler
    {
        private readonly IWhatsappCloudSendMessageServices _whatsappCloudSendMessageServices;
        private readonly ILogger<WhatsAppMessageHandler> _logger;
        private readonly IUtil _util;
        private readonly WhatsAppUtilities _whatsAppUtilities;
        private readonly IWhatsAppMessageRepository _whatsappMessageRepository;
        private readonly IUserServices _userServices;
        private readonly IChatGPTServices _chatGPTServices;
        public WhatsAppMessageHandler(ILogger<WhatsAppMessageHandler> logger,
                                      IUtil util,
                                      IWhatsappCloudSendMessageServices whatsappCloudSendMessageServices, 
                                      WhatsAppUtilities whatsAppUtilities,
                                      IWhatsAppMessageRepository whatsAppMessageRepository,
                                      IUserServices userServices,
                                      IChatGPTServices chatGPTServices)
        {
            _logger = logger;
            _util = util;
            _whatsAppUtilities = whatsAppUtilities;
            _whatsappMessageRepository = whatsAppMessageRepository;
            _whatsappCloudSendMessageServices = whatsappCloudSendMessageServices;
            _userServices = userServices;
            _chatGPTServices = chatGPTServices;
        }

        public async Task<bool> Sample(string phone)
        {
            try
            {
                var data = new[]
                {
                    new
                    {
                        type= "text",
                        text=  "Cesar"
                    },
                    new
                    {
                        type= "text",
                        text=  "Aniversario de Novios"
                    },
                    new
                    {
                        type= "text",
                        text=  "05/06/2023"
                    },
                    new
                    {
                        type= "text",
                        text=  "12:00"
                    },
                };
                //BackgroundJob.Schedule(()=>  _whatsappCloudSendMessage.Execute(data), TimeSpan.FromMinutes(3));
                var objectMessage = _util.TemplateMessage(phone, "recordatorios", data);
                await _whatsappCloudSendMessageServices.Execute(objectMessage);
                //BackgroundJob.Schedule(() => _whatsappCloudSendMessage.Execute(objectMessage), TimeSpan.FromMinutes(3));
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex,ex.StackTrace);
                return false;
            }
        }

        public bool UrlTokenValidator(string token, string challenge)
        {
            if (challenge != null && token != null && _whatsAppUtilities.ValidationTokenUrlWhatsapp(token))
            {
                return true;
            }
            return false;
        }

        public async Task<string> AutoResponse(WhatsAppCloudModel message)
        {
            try
            {
                var whatsappMessage = message.Entry[0].Changes[0].Value.Messages[0];

                if(whatsappMessage == null)
                    throw new ArgumentException("message without text");

                var userNumberPhone = whatsappMessage.From;
                    
                if(userNumberPhone.Length > 12)
                    userNumberPhone = userNumberPhone.Remove(2,1);

                var userData =await _userServices.GetByPhone(userNumberPhone);
                    
                if(userData == null)
                    throw new UnauthorizedAccessException("unregistered user");

                string textMessage = _whatsAppUtilities.GetUserText(whatsappMessage);

                var messageToSave = new WhatsAppMessage{
                    MessageText = textMessage,
                    MessageId = whatsappMessage.Id,
                    Direction = MessageDirection.Incoming,
                    UserId = userData.UserId,
                    Timestamp = DateTime.UtcNow,
                     Status = MessageStatus.Unsent
                };

                var responseChatGPT = await _chatGPTServices.Execute(textMessage, userData.UserId.ToString());
                var objectMessage = _util.TextMessage(responseChatGPT,userNumberPhone);

                await _whatsappCloudSendMessageServices.Execute(objectMessage);
                messageToSave.Status = MessageStatus.Sent;
                await _whatsappMessageRepository.Add(messageToSave);

                return "Response message send sucessefull";

            }
            catch (System.Exception ex )
            {
                
                _logger.LogError(ex,ex.Message);
                return("error for autoresponse");
            }
        }

    }


}