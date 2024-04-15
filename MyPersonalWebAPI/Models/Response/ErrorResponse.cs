using System;

namespace MyPersonalWebAPI.Models.Response
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
