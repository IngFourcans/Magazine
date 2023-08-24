using AutoMapper;
using Magazine.Models;
using Magazine.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Magazine.Controllers
{
    
    public class AvisosController : Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioAvisos repositorioAvisos;
        private readonly IMapper mapper;
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IRepositorioCategorias repositorioCategorias;

        public AvisosController(IRepositorioClientes repositorioClientes,
                                IRepositorioAvisos repositorioAvisos,
                                IMapper mapper,IServiciosUsuarios serviciosUsuarios,
                                IRepositorioCategorias repositorioCategorias)
        {
            this.repositorioClientes = repositorioClientes;
            this.repositorioAvisos = repositorioAvisos;
            this.mapper = mapper;
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioCategorias = repositorioCategorias;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var avisosConClienteYCategoria = await repositorioAvisos.Listar(usuarioId);

            return View(avisosConClienteYCategoria);
        }
        
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var aviso = await repositorioAvisos.AvisosPorId(id, usuarioId);
            
            if (aviso is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<AvisosDTO>(aviso);


            modelo.Clientes = await ObtenerClientes(usuarioId);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(Avisos avisos)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var avisoExiste = await repositorioAvisos.AvisosPorId(avisos.Id,usuarioid);

            if (avisoExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioAvisos.Borrar(avisos);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var aviso = await repositorioAvisos.AvisosPorId(id,usuarioid);
            if (aviso is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }

            var modelo = mapper.Map<AvisosDTO>(aviso);


            modelo.Clientes = await ObtenerClientes(usuarioid);
            modelo.Categorias = await ObtenerCategorias();

            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Avisos avisos)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var avisoExiste = await repositorioAvisos.AvisosPorId(avisos.Id, usuarioid);

            if (avisoExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioAvisos.Actualizar(avisos);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> CrearxCliente(int Id) // es para para enviar datos da la vista del formulario de alta
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new AvisosDTO();

            modelo.Clientes = await ObtenerClientes(Id,usuarioId);
            modelo.Categorias = await ObtenerCategorias();


            return View("Crear", modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Crear() // es para para enviar datos da la vista del formulario de alta
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new AvisosDTO();

            modelo.Clientes = await ObtenerClientes(usuarioId);
            modelo.Categorias = await ObtenerCategorias();


            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(AvisosDTO avisosDTO)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var AvisoCliente = repositorioAvisos.ExisteAvisoxUsuarioxCliente(usuarioid, avisosDTO.Cliente);
            if (AvisoCliente is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }

            if (!ModelState.IsValid)
            {
                avisosDTO.Clientes= await ObtenerClientes(usuarioid);
                avisosDTO.Categorias = await ObtenerCategorias();
               
                return View(avisosDTO);
            }
            //porque aqui no se convierte avisosDTO a avisos, para respetar el tipo de parametro de la funcion crear
            await repositorioAvisos.Crear(avisosDTO);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerClientes(int usuarioId)
        {
            var clientes = await repositorioClientes.Listar(usuarioId);
            return clientes.Select(c => new SelectListItem(c.Empresa, c.Id.ToString()));
            //completo el ienumerable de clientes, LUEGO SE ENVIA como resultado

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerClientes(int cliente,int usuarioId)
        {
            var clientes = await repositorioClientes.Listar(cliente, usuarioId);
            return clientes.Select(c => new SelectListItem(c.Empresa, c.Id.ToString()));
            //completo el ienumerable de clientes, LUEGO SE ENVIA como resultado

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias()
        {
            var categorias = await repositorioCategorias.Listar();
            return categorias.Select(c => new SelectListItem(c.Categoria, c.Id.ToString()));
            //completo el ienumerable de categorias, LUEGO SE ENVIA como resultado

        }
    }
}
