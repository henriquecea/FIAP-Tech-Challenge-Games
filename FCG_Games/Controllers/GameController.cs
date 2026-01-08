using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model;
using FCG_Games.Domain.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetAllAsync() =>
        await gameService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var game = await gameService.GetByIdAsync(id);
        if (game is null) return NotFound();
        return Ok(game);
    }

    [HttpGet("random")]
    public async Task<IActionResult> RandomGameRecomendation() =>
        await gameService.RandomGameRecomendation();

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGameRequest request)
    {
        var created = await gameService.CreateAsync(request);

        return Ok(created);
    }

    [HttpPost("buy/{gameId:guid}")]
    public async Task<IActionResult> BuyGameById(Guid gameId)
    {
        try
        {
            var game = await gameService.BuyGameByIdAsync(gameId);

            return Ok(new
            {
                message = "Compra iniciada com sucesso.",
                gameId = game.Id
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    #region Elastic Search

    [HttpGet("elastic/search")]
    public async Task<IActionResult> SearchGames([FromQuery] string name, [FromQuery] string gender) =>
        Ok(await gameService.SearchGames(name, gender));

    [HttpGet("elastic/suggest")]
    public async Task<IActionResult> SuggestGames([FromQuery] string favoriteGender) =>
        Ok(await gameService.SuggestGamesByHistory(favoriteGender));

    [HttpGet("elastic/top")]
    public async Task<IActionResult> GetMostPopularGames() =>
        Ok(await gameService.GetMostPopularGames());

    #endregion
}

