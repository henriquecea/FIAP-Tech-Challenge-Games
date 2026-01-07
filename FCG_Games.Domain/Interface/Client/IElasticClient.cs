using FCG_Games.Domain.Entity.ElasticSearch;
using FCG_Games.Domain.Model.ElasticSearch;

namespace FCG_Games.Domain.Interface.Client;

public interface IElasticClient<T> where T : LogBaseEntity
{
    Task<IReadOnlyCollection<T>> GetLogs(int page, int size);

    Task<bool> Create(T log);

    #region Game Search

    Task<IReadOnlyCollection<GameElasticDocument>> SearchGames(string text, string gender);

    Task<IReadOnlyCollection<GameElasticDocument>> SuggestGamesByHistory(string favoriteGender);

    Task<Dictionary<string, long>> GetMostPopularGames();

    #endregion
}
