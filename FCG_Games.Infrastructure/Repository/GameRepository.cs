using FCG_Games.Domain.Entity;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Infrastructure.Repository;

public class GameRepository(AppDbContext context) : Repository<GameEntity>(context), IGameRepository
{
    public async Task<GameEntity> GetRandomGameAsync() =>
        await context.Set<GameEntity>()
            .OrderBy(g => Guid.NewGuid())
            .FirstOrDefaultAsync();
}
