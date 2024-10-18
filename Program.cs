using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Infrastructure;
using ControleFinanceiroPessoal.Middlewares;
using ControleFinanceiroPessoal.Repositories;
using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<GenericRepository<Credito>>();
builder.Services.AddScoped<GenericRepository<Debito>>();
builder.Services.AddScoped<GenericRepository<Reserva>>();
builder.Services.AddScoped<GenericRepository<SaldoFinal>>();
builder.Services.AddScoped<GenericRepository<ReservaTransacao>>();
builder.Services.AddScoped<GenericRepository<Usuario>>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<CreditoService>();
builder.Services.AddScoped<DebitoService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<SaldoFinalService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<DashBoardService>();
builder.Services.AddControllers();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp => {
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Adicionar a política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // Substitua pela URL do frontend, ex: React, Angular
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["Key"];
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Controle Financeiro Pessoal API",
        Version = "v1",
        Description = "API para gerenciamento de créditos, débitos e outros aspectos financeiros pessoais.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Maurício Dias de Carvalho Oliveira",
            Email = "mauridf@gmail.com",
        },
    });
});

var app = builder.Build();

// Use a política de CORS
app.UseCors("AllowSpecificOrigin");

// Middleware de exceção global
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
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
