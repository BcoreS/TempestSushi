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
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });

//Aquí se implementan los repositorios
builder.Services.Configure<ImpuestosOptions>(builder.Configuration.GetSection("Impuestos"));
builder.Services.Configure<EnvioOptions>(builder.Configuration.GetSection("Envio"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUsuarioActualService, UsuarioActualService>();
builder.Services.AddScoped<
    IRepositoryReporte,
    RepositoryReporte>();



builder.Services.AddScoped<IRepositoryProcesoPreparacion, RepositoryProcesoPreparacion>();
builder.Services.AddScoped<IRepositoryCombo, RepositoryCombo>();
builder.Services.AddScoped<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddScoped<IRepositoryMenu, RepositoryMenu>();
builder.Services.AddScoped<IRepositoryPedido, RepositoryPedido>();
builder.Services.AddScoped<
    IRepositoryEstacionCocina,
    RepositoryEstacionCocina>();
builder.Services.AddScoped<
    IRepositoryUsuario,
    RepositoryUsuario>();


builder.Services.AddScoped<IServiceProcesoPreparacion, ServiceProcesoPreparacion>();
builder.Services.AddScoped<IServiceCombo, ServiceCombo>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IServicePedido, ServicePedido>();
builder.Services.AddScoped<IServiceClima, ServiceClima>();
builder.Services.AddScoped<
    IServiceAutenticacion,
    ServiceAutenticacion>();
builder.Services.AddScoped<
    IServiceReporte,
    ServiceReporte>();
builder.Services.AddScoped<
    IServiceReportePdf,
    ServiceReportePdf>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ComboProfile>();
    cfg.AddProfile<ProcesoPreparacionProfile>();
    cfg.AddProfile<ProductoProfile>();
    cfg.AddProfile<MenuProfile>();
    cfg.AddProfile<PedidoProfile>();
});

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Services.AddControllersWithViews();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// Config Connection to SQLServer Database
builder.Services.AddDbContext<TempestSushiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

var app = builder.Build();

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