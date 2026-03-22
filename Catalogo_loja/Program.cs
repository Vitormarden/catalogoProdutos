using Catalogo_loja.Data; // Importa sua pasta Data
using Catalogo_loja.Services;
using Catalogo_loja.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura o Banco de Dados SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Configura o CORS (para o React conseguir acessar a API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Porta padrão do Vite/React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Adiciona suporte aos Controllers e FluentValidation (Nova Sintaxe)
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// 4. Configura o AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// 5. Configura o Swagger (Documentação interativa)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

var app = builder.Build();

// Middleware de Exceção Global deve ser um dos primeiros
app.UseMiddleware<Catalogo_loja.Middleware.ExceptionMiddleware>();

// 6. Ativa o Swagger se estiver em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. Ativa o CORS
app.UseCors("ReactPolicy");

app.UseAuthorization();

// 7. Mapeia os Controllers para as rotas
app.MapControllers();

app.Run();