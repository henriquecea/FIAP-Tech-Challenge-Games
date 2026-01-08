using FCG_Games.Application.Service;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Infrastructure.Data;
using FCG_Games.Infrastructure.Repository;
using FCG_Games.WebAPI.Extension;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços customizados
builder.AddSwagger();
builder.AddJwtAuthentication();
builder.AddDbContext();
builder.AddElasticSearch();

// Serviços padrão ASP.NET
builder.Services.AddAuthorization();
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// Injeção de dependência
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Aplicar migrações pendentes ao iniciar a aplicações
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

// Redirecionamento da raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
