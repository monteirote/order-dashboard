using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IOptionsRepository, OptionsRepository>();
builder.Services.AddScoped<IDecorPrintsRepository, DecorPrintsRepository>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MainContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => options.EnableRetryOnFailure()
    )
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=ServiceOrder}/{action=Index}").WithStaticAssets();

app.Run();
