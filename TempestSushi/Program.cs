using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TempestSushi.Application.Profiles;
using TempestSushi.Application.Services;
using TempestSushi.Application.Services.Implementations;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Repository.Implementations;
using TempestSushi.Infraestructure.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using TempestSushi.Application.Options;

var builder = WebApplication.CreateBuilder(args);

// ---------- Autenticación por Cookies ----------
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

// ---------- Opciones de configuración (appsettings.json) ----------
builder.Services.Configure<ImpuestosOptions>(builder.Configuration.GetSection("Impuestos"));
builder.Services.Configure<EnvioOptions>(builder.Configuration.GetSection("Envio"));

// ---------- Accesor de contexto HTTP (requerido por IUsuarioActualService) ----------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActualService, UsuarioActualService>();

// ---------- Repositorios ----------
builder.Services.AddScoped<IRepositoryProcesoPreparacion, RepositoryProcesoPreparacion>();
builder.Services.AddScoped<IRepositoryCombo, RepositoryCombo>();
builder.Services.AddScoped<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddScoped<IRepositoryMenu, RepositoryMenu>();
builder.Services.AddScoped<IRepositoryPedido, RepositoryPedido>();
builder.Services.AddScoped<IRepositoryEstacionCocina, RepositoryEstacionCocina>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();

// ---------- Servicios ----------
builder.Services.AddScoped<IServiceProcesoPreparacion, ServiceProcesoPreparacion>();
builder.Services.AddScoped<IServiceCombo, ServiceCombo>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IServicePedido, ServicePedido>();
builder.Services.AddScoped<IServiceAutenticacion, ServiceAutenticacion>();

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ComboProfile>();
    cfg.AddProfile<ProcesoPreparacionProfile>();
    cfg.AddProfile<ProductoProfile>();
    cfg.AddProfile<MenuProfile>();
    cfg.AddProfile<PedidoProfile>();
});

// ---------- Serilog ----------
var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

builder.Host.UseSerilog(logger);

// ---------- MVC ----------
builder.Services.AddControllersWithViews();

// ---------- Antiforgery (para peticiones AJAX/JSON con header CSRF) ----------
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// ---------- Conexión a SQL Server ----------
builder.Services.AddDbContext<TempestSushiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

var app = builder.Build();

// ---------- Pipeline HTTP ----------
app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
