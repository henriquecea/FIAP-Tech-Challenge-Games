using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCG_Games.Application.Service;

public class GameService(ILogger<GameService> logger,
                         IGameRepository gameRepository) 
    : IGameService
{
    #region CRUD

    public Task<IActionResult> Create(GameModel gameReq)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> DeleteById(int gameId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GameModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GameModel> GetById(int gameId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
