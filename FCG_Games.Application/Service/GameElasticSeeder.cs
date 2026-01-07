using FCG_Games.Domain.Interface.Client;
using FCG_Games.Domain.Model.ElasticSearch;
using FCG_Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Application.Service;

public class GameElasticSeeder(AppDbContext context, 
                               IElasticClient<GameElasticDocument> elasticClient)
{
    public async Task SeedAsync()
    {
        var games = await context.Games.ToListAsync();

        foreach (var game in games)
        {
            await elasticClient.Create(new GameElasticDocument(
                game.Id.ToString(),
                game.Name,
                game.Gender,
                game.Value
            ));
        }
    }
}
