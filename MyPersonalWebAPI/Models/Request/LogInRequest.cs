using System.ComponentModel.DataAnnotations;

namespace MyPersonalWebAPI.Models.Request
{
    public class LogInRequest
    {
        [Required(ErrorMessage = "El campo 'Usuario' es requerido.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo 'Contraseña' es requerido.")]
        public string Password { get; set; }
    }
}