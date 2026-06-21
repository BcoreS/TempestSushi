using Serilog;
using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;

var builder = WebApplication.CreateBuilder(args);

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