using Microsoft.EntityFrameworkCore;
using NovaData;
using NovaBusiness;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar DbContext com Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar serviços da camada Business
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<RecomendacaoService>();
builder.Services.AddScoped<TrilhaService>();

// Configurar Controllers
builder.Services.AddControllers();

// Configurar tratamento de erros com ProblemDetails
builder.Services.AddProblemDetails();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "NOVA Career API", 
        Version = "v1",
        Description = "API para gerenciamento de carreira e recomendações personalizadas"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
