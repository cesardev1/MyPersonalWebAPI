using System;

namespace MyPersonalWebAPI.Models.Response
{
    public class SuccessResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}