using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPersonalWebAPI.Models.Whatsapp
{
    public class WhatsAppMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string MessageId { get; set; }

        [Required]
        [MaxLength(10)]
        public MessageDirection Direction { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public string MessageText { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }

    public enum MessageDirection
    {
        Incoming,
        Outgoing
    }
}