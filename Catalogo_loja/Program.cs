using Catalogo_loja.Data; // Importa sua pasta Data
using Catalogo_loja.Services;
using Microsoft.EntityFrameworkCore;

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

// 3. Adiciona suporte aos Controllers
builder.Services.AddControllers();

// 4. Configura o Swagger (Documentação interativa)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProdutoService, ProdutoService>();

var app = builder.Build();

// 5. Ativa o Swagger se estiver em ambiente de desenvolvimento
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

