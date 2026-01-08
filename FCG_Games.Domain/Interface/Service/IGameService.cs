using FCG_Games.Domain.Model.DTO;
using Microsoft.AspNetCore.Mvc;
namespace FCG_Games.Domain.Interface.Service;

public interface IGameService
{
    /// <summary>
    /// Retrieves all games asynchronously.
    /// </summary>
    /// <returns>A collection of all game models.</returns>
    Task<IActionResult> GetAllAsync();

    /// <summary>
    /// Search a random game recommendation
    /// </summary>
    /// <returns></returns>
    /// 
    Task<GameResponse?> GetByIdAsync(Guid id);

    /// <summary>
    /// Create a new game asynchronously.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GameResponse> CreateAsync(CreateGameRequest request);

    /// <summary>
    /// Choose a random game recommendation
    /// </summary>
    /// <returns></returns>
    Task<IActionResult> RandomGameRecomendation();

    /// <summary>
    /// Make a purchase of a game by its ID
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    Task<GameResponse> BuyGameByIdAsync(Guid gameId);

    #region Elastic Search

    Task<object?> SearchGames(string name, string gender);

    Task<object?> SuggestGamesByHistory(string favoriteGender);

    Task<object?> GetMostPopularGames();

    #endregion
}