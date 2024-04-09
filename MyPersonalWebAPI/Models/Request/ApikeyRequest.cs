using System.ComponentModel.DataAnnotations;

namespace MyPersonalWebAPI.Models.Request
{
    public class ApikeyRequest
    {
        [Required]
        public string Name { get; set; }
    }
}