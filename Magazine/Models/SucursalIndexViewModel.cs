namespace Magazine.Models
{
    public class SucursalIndexViewModel
    {
        public string ClienteNombre { get; set; }
        public IEnumerable<Sucursal> SucursalesxCliente { get; set; }
        
    }
}
