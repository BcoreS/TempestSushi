using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TempestSushi.Application.Profiles;
using TempestSushi.Application.Services.Implementations;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Repository.Implementations;
using TempestSushi.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Aquí se implementan los repositorios

builder.Services.AddScoped<IRepositoryProcesoPreparacion, RepositoryProcesoPreparacion>();
builder.Services.AddScoped<IRepositoryCombo, RepositoryCombo>();
builder.Services.AddScoped<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddScoped<IRepositoryMenu, RepositoryMenu>();
builder.Services.AddScoped<
    IRepositoryEstacionCocina,
    RepositoryEstacionCocina>();


builder.Services.AddScoped<IServiceProcesoPreparacion, ServiceProcesoPreparacion>();
builder.Services.AddScoped<IServiceCombo, ServiceCombo>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ComboProfile>();
    cfg.AddProfile<ProcesoPreparacionProfile>();
    cfg.AddProfile<ProductoProfile>();
    cfg.AddProfile<MenuProfile>();
});

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();