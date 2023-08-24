using AutoMapper;
using Magazine.Models;

namespace Magazine.Servicios
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Avisos,AvisosDTO>();
            CreateMap<Sucursal, SucursalCreacionViewModel>();
            CreateMap<Clientes, ClientesRubrosAsignacionViewModel>();
            
        }
    }
}
