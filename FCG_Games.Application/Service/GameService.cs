using FCG_Games.Domain.Interface.Client;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model;
using FCG_Games.Domain.Model.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCG_Games.Application.Service;

public class GameService(ILogger<GameService> logger,
                         IGameRepository gameRepository,
                         IElasticClient<GameElasticDocument> elasticClient)
    : IGameService
{
    #region CRUD

    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var games = await gameRepository.GetAllAsync();

            return new OkObjectResult(games.Select(user => new
            {
                user.Id,
                user.Name,
                user.Gender,
                user.Value
            }));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar games.");
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> RandomGameRecomendation()
    {
        try
        {
            var game = await gameRepository.GetRandomGameAsync();

            if (game is null)
                return new NotFoundResult();

            return new OkObjectResult(new
            {
                game.Id,
                game.Name,
                game.Gender,
                game.Value
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar game.");
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public Task<IActionResult> BuyGameById(int gameId, PurchaseGameModel purchaseDto)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Elastic

    public async Task<object?> SearchGames(string name, string gender) =>
        await elasticClient.SearchGames(name, gender);

    public async Task<object?> SuggestGamesByHistory(string favoriteGender) =>
        await elasticClient.SuggestGamesByHistory(favoriteGender);

    public async Task<object?> GetMostPopularGames() =>
        await elasticClient.GetMostPopularGames();

    #endregion
}
