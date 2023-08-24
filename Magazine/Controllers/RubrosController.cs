using Dapper;
using Magazine.Models;
using Magazine.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Magazine.Controllers
{
    
    public class RubrosController:Controller
    {
        private readonly IRepositorioRubros repositorioRubros;

        public RubrosController(IRepositorioRubros repositorioRubros)
        {
            this.repositorioRubros = repositorioRubros;
        }
        public IActionResult Crear()
        { 
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Rubros rubros)
        {
            if(!ModelState.IsValid) 
            {
                return View(rubros);
            }

            var YaExisteRubro = await repositorioRubros.Existe(rubros.Rubro.ToString());

            if(YaExisteRubro)  {
                ModelState.AddModelError(nameof(rubros.Rubro), 
                $"El rubro {rubros.Rubro} ya existe.");
                return View(rubros);
            }



            await repositorioRubros.Crear(rubros);    
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> VerificaExistenciaRubro(string rubro)
        {
            var YaExisteRubro = await repositorioRubros.Existe(rubro);
            if(YaExisteRubro)
            {
                return Json($"El rubro {rubro} ya existe.");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var rubros = await repositorioRubros.Listar();
            return View(rubros);
        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var rubro=await repositorioRubros.RubroXId(id);
            if (rubro is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(rubro);
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar(Rubros rubros)
        {
            var rubroExiste = await repositorioRubros.RubroXId(rubros.Id);
            if (rubroExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioRubros.Borrar(rubros);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var rubro = await repositorioRubros.RubroXId(id);
            if (rubro is null ) 
            {
                return RedirectToAction("NoEncontrado","Home");
            }
            return View(rubro);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Rubros rubros)
        {
            var rubroExiste = repositorioRubros.RubroXId(rubros.Id);

            if (rubroExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioRubros.Actualizar (rubros);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
           
            if (ids.Count() <= 0) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var rubrosOrdenados = ids.Select((valor, indice) =>
                                    new Rubros() { Id = valor, Orden = indice + 1 }).AsEnumerable();
            await repositorioRubros.Ordenar(rubrosOrdenados);

            return Ok();
        }




    }
}
