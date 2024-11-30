using ApiLogin.Data;
using ApiLogin.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ApiLogin.Interfaces;
using ApiLogin.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("Startup/appsettings.json", optional: false, reloadOnChange: true);

// Database Context
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Services
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders(); // UserManager do Identity

// Controllers and Endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Login Api", Version = "v1" });
});

// Build the application
var app = builder.Build();

// Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Seed Roles
using (var scope = app.Services.CreateScope()) 
{
    var services = scope.ServiceProvider; 
    var seeder = services.GetRequiredService<IDataSeeder>(); 
    await seeder.SeedRolesAsync(services);
}

// Run the application
app.Run();
