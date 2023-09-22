using Dapper;
using Magazine.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace Magazine.Controllers
{
    
    public class UsuariosController: Controller
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly SignInManager<Usuarios> signInManager;
       
        public UsuariosController(UserManager<Usuarios> userManager,SignInManager<Usuarios> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            
        }

        

       
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro() 
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var usuario = new Usuarios() { Email = model.Email };
            // Crea al usuario en la base de datos
            var resultado = await userManager.CreateAsync(usuario, password: model.Password);

            if (resultado.Succeeded)
            {
               
                await signInManager.SignInAsync(usuario,isPersistent: true);

                // Asigna el rol "UsuarioNuevo" al usuario recién creado
                await userManager.AddToRoleAsync(usuario, "Usuario Comun");

                return RedirectToAction("Index", "Clientes");


            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
                return View(model);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Clientes");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var resultado = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, 
                                                                loginViewModel.Recuerdame, lockoutOnFailure: false);
            if(resultado.Succeeded)
            {
                return RedirectToAction("Index", "Clientes");
            }else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(loginViewModel);
            }
        }

    }
}
