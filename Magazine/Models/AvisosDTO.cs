using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magazine.Models
{
    public class AvisosDTO: Avisos
    {
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
       
    }
}
