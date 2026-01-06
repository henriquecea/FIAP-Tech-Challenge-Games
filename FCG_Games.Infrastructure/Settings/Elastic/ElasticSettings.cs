using FCG_Games.Domain.Interface.Settings;

namespace FCG_Games.Infrastructure.Settings.Elastic;

public class ElasticSettings : IElasticSettings
{
    public string Uri => "http://localhost:9200";
}
