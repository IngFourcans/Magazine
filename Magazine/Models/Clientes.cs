using Magazine.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        public string Empresa { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "El formato del correo electrónico es inválido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        public string Instagram { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [RegularExpression(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", ErrorMessage = "El formato de la dirección web es inválido.")]
        public string Web { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        public string Facebook { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        public string Linkedin { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        public string Twitter { get; set; }
        [StringLength(maximumLength: 13, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [RegularExpression(@"^\d{2}-\d{8}-\d{1}$", ErrorMessage = "El formato del CUIT es inválido.")]
        public string CUIT { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }
        [StringLength(maximumLength: 250, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [Display(Name = "Domicilio Legal")]
        public string DomicilioLegal { get; set; }
        [StringLength(maximumLength: 120, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre Referente")]
        public string ReferenteNombre { get; set; }
        [StringLength(maximumLength: 10, ErrorMessage = "El largo maximo del campo{0} es de {1} caracteres.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El formato del número de celular es inválido.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Celular Referente")]
        public string CelularReferente { get; set; }
        public int aviso { get; set; }
    }
}
