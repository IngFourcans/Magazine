using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Magazine.Models
{
    public class SucursalCreacionViewModel: Sucursal
    {
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public IEnumerable<SelectListItem> Provincias { get; set; } 
        public IEnumerable<SelectListItem> Localidades { get; set; }
        [Display(Name = "Es sucursal central")]
        public bool EsSucursalCentral => SucursalCentral != 0;



    }
}
