using AutoMapper;
using Magazine.Models;
using Magazine.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;

namespace Magazine.Controllers
{
    
    public class ClientesController: Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IMapper mapper;

        public ClientesController(IRepositorioClientes repositorioClientes,IServiciosUsuarios serviciosUsuarios, IMapper mapper)
        {
            this.mapper = mapper;
            this.repositorioClientes = repositorioClientes;
            this.serviciosUsuarios = serviciosUsuarios;
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            
            if (usuarioId == 0)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var clientes = await repositorioClientes.Listar(usuarioId);
            return View(clientes);
            
        }
        [HttpGet]
        public IActionResult Crear()
        {
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Clientes clientes) 
        {
            if (!ModelState.IsValid)
            {
                return View(clientes);
            }
            await repositorioClientes.Crear(clientes);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var cliente = await repositorioClientes.ClienteXId(id, usuarioId);
            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            
            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Clientes cliente)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var clienteExiste = await repositorioClientes.ClienteXId(cliente.Id, usuarioid);

            if (clienteExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioClientes.Actualizar(cliente);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var cliente = await repositorioClientes.ClienteXId(id, usuarioId);
            if (cliente is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(Clientes cliente)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var clienteExiste = await repositorioClientes.ClienteXId(cliente.Id, usuarioid);

            if (clienteExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioClientes.Borrar(cliente.Id);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> RubrosAsignados(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var clientex = await repositorioClientes.ClienteXId(id, usuarioId);  

            if (clientex is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = mapper.Map<ClientesRubrosAsignacionViewModel>(clientex);

             /*var modelo = new ClientesRubrosAsignacionViewModel()
             {
                 Id= clientex.Id,
                 Empresa= clientex.Empresa
                

             };*/

             modelo.Rubros = await repositorioClientes.ObtenerRubrosxCliente(id);

            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> RubrosAsignados(List<int> rubrosseleccionados, int id)
        {
            var usuarioid = serviciosUsuarios.ObtenerUsuarioId();
            var clienteExiste = await repositorioClientes.ClienteXId(id, usuarioid);

            if (clienteExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }


            await repositorioClientes.ActualizarRubrosSeleccionados(rubrosseleccionados,id);

            return RedirectToAction("Index");
        }
    }
}
