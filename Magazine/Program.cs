using Magazine.Models;
using Magazine.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container


var PoliticaUsuarioAuntenticado = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(PoliticaUsuarioAuntenticado));
});
/*revisar ->*/
/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("Vendedor", policy => policy.RequireRole("Vendedor"));
    options.AddPolicy("Consulta", policy => policy.RequireRole("Consulta"));
    options.AddPolicy("Usuario Comun", policy => policy.RequireRole("Usuario Comun"));

});*/

builder.Services.AddTransient<IRepositorioRubros, RepositorioRubros>();
builder.Services.AddTransient<IRepositorioClientes, RepositorioClientes>();
builder.Services.AddTransient<IRepositorioAvisos, RepositorioAvisos>();
builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioProvincias , RepositorioProvincias>();
builder.Services.AddTransient<IRepositorioSucursales, RepositorioSucursales>();
builder.Services.AddTransient<IRepositorioLocalidades, RepositorioLocalidades>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IUserStore<Usuarios>, UsuariosStore>();

builder.Services.AddTransient<SignInManager<Usuarios>>();
builder.Services.AddIdentityCore<Usuarios>(opciones =>
{
    opciones.Password.RequireNonAlphanumeric = false;
    //opciones.Password.RequireUppercase = false;
    //opciones.Password.RequireLowercase = false;
}
).AddErrorDescriber<MensajesDeErrorIdentity>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    option.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    option.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
}
).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/Usuarios/login";
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
