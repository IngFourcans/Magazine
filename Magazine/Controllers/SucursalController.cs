using AutoMapper;
using Magazine.Models;
using Magazine.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magazine.Controllers
{
    
    public class SucursalController: Controller
    {
        
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioProvincias repositorioProvincias;
        private readonly IRepositorioLocalidades repositorioLocalidades;
        private readonly IRepositorioSucursales repositorioSucursales;
        private readonly IMapper mapper;

        public SucursalController(IServiciosUsuarios serviciosUsuarios,IRepositorioClientes repositorioClientes
                                  ,IRepositorioProvincias repositorioProvincias, IRepositorioLocalidades repositorioLocalidades,
                                    IRepositorioSucursales repositorioSucursales, IMapper mapper) 
        {
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioClientes = repositorioClientes;
            this.repositorioProvincias = repositorioProvincias;
            this.repositorioLocalidades = repositorioLocalidades;
            this.repositorioSucursales = repositorioSucursales;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var sucursal = await repositorioSucursales.ObtenerSucursalxId(id, usuarioId);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(sucursal);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarSucursal(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var sucursal = await repositorioSucursales.ObtenerSucursalxId(id, usuarioId);

            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioSucursales.Borrar(id);
            return RedirectToAction("Index", "Sucursal", new { id = sucursal.Cliente });

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var sucursal = await repositorioSucursales.ObtenerSucursalxId(id,usuarioId);
            
            if (sucursal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            
            var modelo = mapper.Map<SucursalCreacionViewModel>(sucursal);
            
            modelo.Provincias = await ObtenerProvincias();
            modelo.Localidades = await ObtenerLocalidades(modelo.Provincia);

            

            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(SucursalCreacionViewModel sucursal)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var sucursalexiste = await repositorioSucursales.ObtenerSucursalxId(sucursal.Id,usuarioid);

            if (sucursalexiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioSucursales.Editar(sucursal);
            return RedirectToAction("Index","Sucursal", new { id = sucursal.Cliente});

        }
        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new SucursalCreacionViewModel();

            modelo.Clientes = await ObtenerClientes(usuarioId);
            modelo.Provincias = await ObtenerProvincias();
            modelo.Localidades = await ObtenerLocalidades(modelo.Provincia);



            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Crear (SucursalCreacionViewModel modelo)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            if (!ModelState.IsValid)
            {
                modelo.Clientes = await ObtenerClientes(usuarioid);
                modelo.Provincias = await ObtenerProvincias();
                modelo.Localidades = await ObtenerLocalidades(modelo.Provincia);

                return View(modelo);
            }

            var ClienteExiste = repositorioClientes.ClienteXId(modelo.Id,usuarioid);
            if (ClienteExiste is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioSucursales.Crear(modelo);
            return RedirectToAction("Index", "Sucursal", new { id = modelo.Cliente });

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerClientes(int usuarioId)
        {
            var clientes = await repositorioClientes.Listar(usuarioId);
            return clientes.Select(c => new SelectListItem(c.Empresa, c.Id.ToString()));
            //completo el ienumerable de clientes, LUEGO SE ENVIA como resultado

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerProvincias()
        {
            var provincias = await repositorioProvincias.Listar();
            return provincias.Select(c => new SelectListItem(c.Provincia, c.Id.ToString()));
            //completo el ienumerable de clientes, LUEGO SE ENVIA como resultado

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerLocalidades(int provincia)
        {
            var Localidades = await repositorioLocalidades.Listar(provincia);
            return Localidades.Select(c => new SelectListItem(c.Localidad, c.Id.ToString()));
            
        }
        [HttpPost]
        public async Task<IActionResult> ObtenerLocalidadespost([FromBody] int provincia) 
        {
            var localidades = await ObtenerLocalidades(provincia);
            return Ok(localidades);
        }



        public async Task<IActionResult> Index(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var sucursalesxCliente = await repositorioSucursales.Listar(id,usuarioId);
            var modelo = sucursalesxCliente
                .GroupBy(x => x.ClienteNombre)
                .Select(grupo => new SucursalIndexViewModel
                {
                    ClienteNombre=grupo.Key,
                    SucursalesxCliente = grupo.AsEnumerable()
                }).ToList();

            return View(modelo);
        }

    
    }
}
