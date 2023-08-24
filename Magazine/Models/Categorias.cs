using Magazine.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class Categorias
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EnMayusculas]
        public string Categoria { get; set; }
    }
}
