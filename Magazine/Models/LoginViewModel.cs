using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El formato de email no es correcto.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Recuerdame { get; set; }
    }
}
