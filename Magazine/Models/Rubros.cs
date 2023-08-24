using Magazine.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class Rubros//: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(maximumLength:50,ErrorMessage ="El largo maximo del campo{0} es de {1} caracteres.")]
        [Display(Name ="Nombre del Rubro")]
        [EnMayusculas]
        [Remote(action: "VerificaExistenciaRubro", controller: "Rubros")]
        public string Rubro { get; set; }
        public int Orden { get; set; }

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    if (Rubro != null && Rubro.Length > 0) {
        //        if(Rubro.ToString() != Rubro.ToString().ToUpper()) 
        //        {
        //            yield return new ValidationResult("El rubro se debe escribir con letras MAYUSCULAS",
        //                new[] {nameof(Rubro)});
        //        }
        //    }
        //}
    }


}
