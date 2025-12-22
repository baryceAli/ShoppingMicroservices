using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ShoppingMicroservices.Services.AuthAPI.Data;
using ShoppingMicroservices.Services.AuthAPI.Models;
using ShoppingMicroservices.Services.AuthAPI.Service;
using ShoppingMicroservices.Services.AuthAPI.Service.IService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTockenGenerator, JwtTockenGenerator>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


applyMigration();

app.Run();

void applyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_context.Database.GetPendingMigrations().Any())
        {
            _context.Database.Migrate();
        }
    }
}


