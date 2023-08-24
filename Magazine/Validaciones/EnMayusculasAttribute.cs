using System.ComponentModel.DataAnnotations;

namespace Magazine.Validaciones
{
    public class EnMayusculasAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            if (value.ToString() != value.ToString().ToUpper() ) 
            {
                return new ValidationResult("El texto debe estar escrito con letras MAYUSCULAS.");
            }

            return ValidationResult.Success;
        }
    }
}
