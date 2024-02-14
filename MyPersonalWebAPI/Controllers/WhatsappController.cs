using Microsoft.AspNetCore.Mvc;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.OpenIA.ChatGPT;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;
using MyPersonalWebAPI.Util;

namespace MyPersonalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsappController : ControllerBase
    {
        private readonly IWhatsappCloudSendMessageServices _whatsappCloudSendMessageServices;
        private readonly IUtil _util;
        private readonly ILogger<WhatsappController> _logger;
        private readonly IChatGPTServices _chatGPTServices;
        

        public WhatsappController(IWhatsappCloudSendMessageServices whatsappCloudSendMessageServices,
                                  IUtil util,
                                  ILogger<WhatsappController> logger,
                                  IChatGPTServices chatGPTServices)
        {
            _logger = logger;
            _util = util;
            _whatsappCloudSendMessageServices = whatsappCloudSendMessageServices;
            _chatGPTServices = chatGPTServices;
        }

        [HttpGet("test/{phone}")]
        public async Task<IActionResult> Sample(string phone)
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

            return Ok(" ok sample");
        }


        [HttpGet]
        public IActionResult VerifyToken()
        {
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();


            //TODO: this can be refactored by or's so that only one conditional remains
            if (challenge != null && token != null && _whatsappCloudSendMessageServices.ValidationTokenUrlWhatsapp(token))
            {
                return Ok(challenge);
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RecivedMessage([FromBody] WhatsAppCloudModel body)
        {

            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];

                if (Message != null)
                {
                    var userNumber = Message.From;
                    _logger.LogInformation("new message from:" + userNumber);
                    if (userNumber.Length > 12)
                        userNumber = userNumber.Remove(2, 1);
                    var userText = _whatsappCloudSendMessageServices.GetUserText(Message);

                    var responseChatGPT = await _chatGPTServices.Execute(userText, userNumber);
                    var objectMesage = _util.TextMessage(responseChatGPT, userNumber);


                    await _whatsappCloudSendMessageServices.Execute(objectMesage);
                }
                return Ok("EVENT_RECEIVED");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok("EVENT_RECEIVED");
            }
        }
    }
}
