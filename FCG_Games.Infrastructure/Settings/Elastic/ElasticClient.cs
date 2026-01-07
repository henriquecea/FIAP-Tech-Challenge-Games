using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.Nodes;
using FCG_Games.Domain.Entity.ElasticSearch;
using FCG_Games.Domain.Interface.Client;
using FCG_Games.Domain.Model.ElasticSearch;

namespace FCG_Games.Infrastructure.Settings.Elastic;

public class ElasticClient<T>(ElasticsearchClient client)
    : IElasticClient<T> where T : LogBaseEntity
{
    private static readonly IndexName Index =
        typeof(T).Name.ToLowerInvariant();

    public async Task<IReadOnlyCollection<T>> GetLogs(int page, int size)
    {
        var response = await client.SearchAsync<T>(s => s
            .Indices(Index)
            .From((page - 1) * size)
            .Size(size)
        );

        return response.Documents;
    }

    public async Task<bool> Create(T log)
    {
        var response = await client.IndexAsync(log, i => i.Index(Index));

        if (!response.IsValidResponse)
            return false;

        return true;
    }

    public async Task<IReadOnlyCollection<GameElasticDocument>> SearchGames(string text, string gender)
    {
        var response = await client.SearchAsync<GameElasticDocument>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Match(mm => mm
                            .Field(f => f.Name)
                            .Query(text)
                        )
                    )
                    .Filter(f => f
                        .Term(t => t
                            .Field("gender.keyword")
                            .Value(gender)
                        )
                    )
                )
            )
        );

        return response.Documents;
    }

    public async Task<IReadOnlyCollection<GameElasticDocument>> SuggestGamesByHistory(string favoriteGender)
    {
        var response = await client.SearchAsync<GameElasticDocument>(s => s
            .Size(10)
            .Query(q => q
                .Term(t => t
                    .Field("gender.keyword")
                    .Value(favoriteGender)
                )
            )
        );

        return response.Documents;
    }

    public async Task<Dictionary<string, long>> GetMostPopularGames()
    {
        var response = await client.SearchAsync<GameElasticDocument>(s => s
            .Size(0)
            .Aggregations(a => a
                .Add("top_games", agg => agg
                    .Terms(t => t
                        .Field("name.keyword")
                        .Size(10)
                    )
                )
            )
        );

        if (!response.Aggregations.TryGetValue("top_games", out var aggregate))
            return [];

        var termsAgg = aggregate as StringTermsAggregate;

        if (termsAgg?.Buckets == null)
            return [];

        return termsAgg.Buckets.ToDictionary(
            b => b.Key.ToString(),
            b => b.DocCount
        );
    }
}
