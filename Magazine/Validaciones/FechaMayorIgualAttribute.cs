using System.ComponentModel.DataAnnotations;

namespace Magazine.Validaciones
{
    public class FechaMayorIgualAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is null))
            { 
                DateTime fecha = (DateTime)value;
                DateTime fechaActual = DateTime.Now.Date;

                if (fecha < fechaActual)
                {
                    return new ValidationResult("La fecha debe ser mayor o igual a la fecha actual.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
