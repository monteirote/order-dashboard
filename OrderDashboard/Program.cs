using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderDashboard.Database;
using OrderDashboard.Repositories;
using OrderDashboard.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IOptionsRepository, OptionsRepository>();
builder.Services.AddScoped<IDecorPrintsRepository, DecorPrintsRepository>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MainContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => options.EnableRetryOnFailure()
    )
);

// ----- SUBSTITUA SEU BLOCO DE AUTENTICAÇÃO POR ESTE -----

builder.Services.AddAuthentication(options =>
{
    // 1. Define o esquema de Cookies como o PADRÃO para o site.
    // Isso garante que [Authorize] em controllers de páginas (como ServiceOrder)
    // irá acionar o redirecionamento para o login.
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
// 2. Configura o manipulador de Cookies.
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);

    // Esta parte está correta e é muito útil!
    // Ela impede o redirecionamento em chamadas de API, retornando 401.
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/Dashboard"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
})
// 3. Configura o manipulador de JWT (Bearer Token).
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""))
    };
});

// -----------------------------------------------------------

var app = builder.Build();

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

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=ServiceOrder}/{action=Index}").WithStaticAssets();

app.Run();
