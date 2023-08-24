using Magazine.Validaciones;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;


namespace Magazine.Models
{
    public class Avisos
    {
        public int Id { get; set; }
        [StringLength(maximumLength:60)]
        [Display(Name ="Titulo SEO")]
        public string TituloSEO { get; set; }
        [StringLength(maximumLength: 160)]
        [Display(Name = "Descripcion SEO")]
        public string DescripcionSEO { get; set; }
        [StringLength(maximumLength: 250)]
        [Display(Name = "Ubicacion Imagen")]
        public string RutaImagen { get; set; }
        [Display(Name = "Fecha Actualizacion")]
        [DataType(DataType.Date)]
        public DateTime FechaActualizacion{ get; set; } = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
        [Display(Name = "Vigente Hasta ")]
        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; } 
        [Display(Name = "Fecha Baja")]
        [FechaMayorIgual]
        [DataType(DataType.Date)]
        public DateTime? FechaBaja { get; set; } 
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Cliente { get; set; }
        public string NombreCliente { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Categoria { get; set; }
        public string NombreCategoria { get; set; }
        


    }

}
