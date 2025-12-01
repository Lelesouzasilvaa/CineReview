using CineReview.Api.Data;
using CineReview.Api.Services;
using CineReview.Api.Services.Implementations;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adiciona controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra serviços corretos
builder.Services.AddScoped<IAuthServices, AuthService>();
builder.Services.AddScoped<IMidiaService, MidiaService>();
builder.Services.AddScoped<IApiExternaService, ApiExternaService>();

// Configura EF Core com SQLite
builder.Services.AddDbContext<CineReviewContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CONFIGURAÇÃO DO JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("Jwt:Key não está configurado no appsettings.json");

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddHttpClient();

// Cria o app
var app = builder.Build();

// Configura o pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
