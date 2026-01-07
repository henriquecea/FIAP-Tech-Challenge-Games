using FCG_Games.Application.Service;
using FCG_Games.Domain.Interface.Client;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model.ElasticSearch;
using FCG_Games.Infrastructure.Data;
using FCG_Games.Infrastructure.Repository;
using FCG_Games.Infrastructure.Settings.Elastic;
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

builder.Services.AddScoped<GameElasticSeeder>();
builder.Services.AddScoped<IElasticClient<GameElasticDocument>, ElasticClient<GameElasticDocument>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Aplicar migrações pendentes ao iniciar a aplicações
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Popular o ElasticSearch com os dados do banco relacional
    var seeder = scope.ServiceProvider.GetRequiredService<GameElasticSeeder>();
    await seeder.SeedAsync();
}

app.UseSwagger();
app.UseSwagger();

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
