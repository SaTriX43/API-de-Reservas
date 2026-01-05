using API_de_Reservas.DALs;
using API_de_Reservas.DALs.RecursoRepositoryCarpeta;
using API_de_Reservas.DALs.ReservaRepositoryCarpeta;
using API_de_Reservas.DALs.UsuarioRepositoryCarpeta;
using API_de_Reservas.Middleware;
using API_de_Reservas.Models;
using API_de_Reservas.Services.AutenticacionServiceCarpeta;
using API_de_Reservas.Services.JwtServiceCarpeta;
using API_de_Reservas.Services.RecursoServiceCarpeta;
using API_de_Reservas.Services.ReservaServiceCarpeta;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// SERILOG
// =======================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// =======================
// DATABASE
// =======================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =======================
// AUTHENTICATION (JWT)
// =======================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =======================
// SERVICES
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IRecursoService, RecursoService>();
builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();


builder.Services.AddScoped<IReservasService, ReservasService>();
builder.Services.AddScoped<IReservasRepository, ReservasRepository>();

builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

// =======================
// APP
// =======================


var app = builder.Build();

// =======================
// MIDDLEWARES
// =======================
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
