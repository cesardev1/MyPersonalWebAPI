using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyPersonalWebAPI.Models;

namespace MyPersonalWebAPI.Auth
{
    public class ApiKey
    {
        [Key]
        public Guid ApiKeyId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public DateTime DateAtCreated { get; set; }
        public DateTime DateAtUpdated { get; set; }
        public Status CurrentStatus { get; set; }
        public Guid UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
    public enum Status
    {
        Active,
        Inactive,
        Pending,
        Suspend,
        Paused
    }
}