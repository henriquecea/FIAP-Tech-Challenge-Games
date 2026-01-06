using FCG_Games.Domain.Entity;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Infrastructure.Data;

namespace FCG_Games.Infrastructure.Repository;

public class GameRepository(AppDbContext context) : Repository<GameEntity>(context), IGameRepository
{

}
