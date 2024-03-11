
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPersonalWebAPI.Models
{
    [Index(nameof(Username),IsUnique =true)]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50,MinimumLength = 3,ErrorMessage ="El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Inglesa un correo electonico valido.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Inglese un numero de telefono valido.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "El Rol es Obligatorio")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
    }
}
