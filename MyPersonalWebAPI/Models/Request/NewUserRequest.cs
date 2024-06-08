using System.ComponentModel.DataAnnotations;

namespace MyPersonalWebAPI.Models.Request
{
    public class NewUserRequest
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
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
        public int[] Roles { get; set; }
    }
}
