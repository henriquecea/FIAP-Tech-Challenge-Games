using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public Task<IActionResult> Create(GameModel gameReq)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete()]
    public Task<IActionResult> DeleteById(int gameId)
    {
        throw new NotImplementedException();
    }

    [HttpGet()]
    public Task<IEnumerable<GameModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{gameId}")]
    public Task<GameModel> GetById(int gameId)
    {
        throw new NotImplementedException();
    }
}
