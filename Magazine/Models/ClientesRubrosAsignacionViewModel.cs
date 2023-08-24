using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magazine.Models
{
    public class ClientesRubrosAsignacionViewModel: Clientes
    {
        
        public IEnumerable<RubrosSeleccionados> Rubros { get; set; }

    }
}
