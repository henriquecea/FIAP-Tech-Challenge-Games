using FCG_Games.Domain.Configuration;
using FCG_Games.Domain.Entity.ElasticSearch;

namespace FCG_Games.Domain.Model.ElasticSearch;

[ElasticIndex("games")]
public class GameElasticDocument : LogBaseEntity
{
    public string Gender { get; set; }
    public decimal Value { get; set; }

    public GameElasticDocument(
        string id,
        string name,
        string gender,
        decimal value
    ) : base(id, name)
    {
        Gender = gender;
        Value = value;
    }

    protected GameElasticDocument() : base(string.Empty, string.Empty)
    {
    }
}