using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class Sucursal
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 250)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombre de la sucursal")]
        public string NombreSucursal { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Es la sucursal central?")]
        public int SucursalCentral { get; set; }
        [StringLength(maximumLength: 250)]
        public string Calle { get; set; }
        [StringLength(maximumLength: 10)]
        public string Nro { get; set; }
        [StringLength(maximumLength: 10)]
        public string Piso { get; set; }
        [StringLength(maximumLength: 10)]
        public string Departamento { get; set; }
        [StringLength(maximumLength: 250)]
        [Display(Name = "Información Adicional")]
        public string InformacionAdicional { get; set; }
        [StringLength(maximumLength: 18)]
        public string Telefono { get; set; }
        [StringLength(maximumLength: 18)]
        public string Whatsapp { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Provincia { get; set; } = 1;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Localidad { get; set; } = 1;
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Cliente { get; set; }
        public string ClienteNombre { get; set; }
        public string ProvinciaNombre { get; set; }
        public string LocalidadNombre { get; set; }
    }
}
