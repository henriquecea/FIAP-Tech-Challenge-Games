using FCG_Games.Domain.Entity;

namespace FCG_Games.Domain.Interface.Repository;

public interface IGameRepository : IRepository<GameEntity>
{
    Task<GameEntity> GetRandomGameAsync();
}
