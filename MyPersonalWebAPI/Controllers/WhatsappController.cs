using Microsoft.AspNetCore.Mvc;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Models.Whatsapp;
using MyPersonalWebAPI.Services.OpenIA.ChatGPT;
using MyPersonalWebAPI.Services.WhatsappClound;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;
using MyPersonalWebAPI.Util;

namespace MyPersonalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsappController : ControllerBase
    {


        private readonly ILogger<WhatsappController> _logger;

        private readonly IWhatsAppMessageHandler _whatsAppMessageHandler;
        public WhatsappController(ILogger<WhatsappController> logger,
                                  IWhatsAppMessageHandler whatsAppMessageHandler)
        {
            _logger = logger;
            _whatsAppMessageHandler = whatsAppMessageHandler;
        }

        [HttpGet("test/{phone}")]
        public async Task<IActionResult> Sample(string phone)
        {
            await _whatsAppMessageHandler.Sample(phone);
            return Ok();
        }


        [HttpGet]
        public IActionResult VerifyToken()
        {
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();


            //TODO: this can be refactored by or's so that only one conditional remains
            if (challenge != null && token != null && _whatsAppMessageHandler.UrlTokenValidator(token, challenge))
            {
                return Ok(challenge);
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RecivedMessage([FromBody] WhatsAppCloudModel body)
        {
            var response = await _whatsAppMessageHandler.AutoResponse(body);
            return Ok(response);
        }
    }
}
