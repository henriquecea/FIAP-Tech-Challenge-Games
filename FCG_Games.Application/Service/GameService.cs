using FCG_Games.Domain.Entity;
using FCG_Games.Domain.Interface.Client;
using FCG_Games.Domain.Interface.Repository;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model.DTO;
using FCG_Games.Domain.Model.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCG_Games.Application.Service;

public class GameService(
    ILogger<GameService> logger,
    IGameRepository gameRepository,
    IGameMessageService gameMessageService,
    IElasticClient<GameElasticDocument> elasticClient)
    : IGameService
{
    #region CRUD

    public async Task<IActionResult> GetAllAsync()
    {
        logger.LogInformation("Iniciando GetAllAsync");

        try
        {
            var games = await gameRepository.GetAllAsync();
            logger.LogInformation("GetAllAsync retornou {TotalGames} games", games.Count());

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

    public async Task<GameResponse?> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Iniciando GetByIdAsync para Id {GameId}", id);

        try
        {
            var game = await gameRepository.GetByIdAsync(id);

            if (game is null)
            {
                logger.LogWarning("GetByIdAsync: Game {GameId} não encontrado", id);
                return null;
            }

            logger.LogInformation("GetByIdAsync: Game {GameId} encontrado", id);

            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Gender = game.Gender,
                Value = game.Value
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar game por Id {GameId}", id);
            throw;
        }
    }

    public async Task<IActionResult> RandomGameRecomendation()
    {
        logger.LogInformation("Iniciando RandomGameRecomendation");

        try
        {
            var game = await gameRepository.GetRandomGameAsync();

            if (game is null)
            {
                logger.LogWarning("RandomGameRecomendation: Nenhum game encontrado");
                return new NotFoundResult();
            }

            logger.LogInformation("RandomGameRecomendation retornou game {GameId}", game.Id);

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
            logger.LogError(ex, "Erro ao buscar game aleatório");
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<GameResponse> CreateAsync(CreateGameRequest request)
    {
        logger.LogInformation("Iniciando CreateAsync para game {GameName}", request.Name);

        try
        {
            var game = new GameEntity(request.Name, request.Gender)
            {
                Id = Guid.NewGuid(),
                Value = request.Value
            };

            await gameRepository.AddAsync(game);
            await gameRepository.SaveChangesAsync();

            logger.LogInformation("CreateAsync: Game {GameId} criado com sucesso", game.Id);

            return new GameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Gender = game.Gender,
                Value = game.Value
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao criar game {GameName}", request.Name);
            throw; // deixa o controller tratar
        }
    }

    public async Task<GameResponse> BuyGameByIdAsync(Guid gameId)
    {
        logger.LogInformation("Iniciando BuyGameByIdAsync para game {GameId}", gameId);

        try
        {
            var game = await GetByIdAsync(gameId);

            if (game is null)
            {
                logger.LogWarning("BuyGameByIdAsync: Game {GameId} não encontrado", gameId);
                throw new KeyNotFoundException("Game não encontrado.");
            }

            await gameMessageService.SendMessageAsync(game);
            logger.LogInformation("BuyGameByIdAsync: Game {GameId} comprado com sucesso", gameId);

            return game;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao comprar game {GameId}", gameId);
            throw;
        }
    }

    #endregion

    #region Elastic

    public async Task<object?> SearchGames(string name, string gender)
    {
        logger.LogInformation("Iniciando SearchGames com Name={Name} e Gender={Gender}", name, gender);

        try
        {
            var result = await elasticClient.SearchGames(name, gender);
            logger.LogInformation("SearchGames retornou {Count} resultados", (result as IEnumerable<object>)?.Count() ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar games no ElasticSearch");
            throw;
        }
    }

    public async Task<object?> SuggestGamesByHistory(string favoriteGender)
    {
        logger.LogInformation("Iniciando SuggestGamesByHistory para Gender={Gender}", favoriteGender);

        try
        {
            var result = await elasticClient.SuggestGamesByHistory(favoriteGender);
            logger.LogInformation("SuggestGamesByHistory retornou {Count} sugestões", (result as IEnumerable<object>)?.Count() ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao sugerir games pelo histórico");
            throw;
        }
    }

    public async Task<object?> GetMostPopularGames()
    {
        logger.LogInformation("Iniciando GetMostPopularGames");

        try
        {
            var result = await elasticClient.GetMostPopularGames();
            logger.LogInformation("GetMostPopularGames retornou {Count} games populares", (result as IEnumerable<object>)?.Count() ?? 0);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao buscar games mais populares");
            throw;
        }
    }

    #endregion
}
