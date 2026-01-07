using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetAllAsync() =>
        await gameService.GetAllAsync();

    [HttpGet("random")]
    public async Task<IActionResult> RandomGameRecomendation() =>
        await gameService.RandomGameRecomendation();

    [HttpPost("buy/{gameId}")]
    public async Task<IActionResult> BuyGameById(int gameId, [FromBody] PurchaseGameModel purchaseDto) =>
        await gameService.BuyGameById(gameId, purchaseDto);

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
