using Microsoft.AspNetCore.Mvc;
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
        private ILogger<WhatsappController> _logger;
        

        public WhatsappController(IWhatsappCloudSendMessageServices whatsappCloudSendMessageServices,
                                  IUtil util,
                                  ILogger<WhatsappController> logger)
        {
            _logger = logger;
            _util = util;
            _whatsappCloudSendMessageServices = whatsappCloudSendMessageServices;
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
    }
}
