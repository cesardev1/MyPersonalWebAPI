namespace MyPersonalWebAPI.Models
{
    public class SecretsOptions
    {
        public string WhatsappPhoneId { get; set; }
        public string ApiKeyWhatsapp { get; set; }
        public string ApiKeyOpenIA { get; set; }
        public string AccessTokenWhatsapp { get; set; }
        public string JWTSecretKey { get; set; }
        public string PostgreConnectionString { get; set; }
    }
}
