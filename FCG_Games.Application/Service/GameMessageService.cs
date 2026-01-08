using Azure.Messaging.ServiceBus;
using FCG_Games.Domain.Interface.Service;
using FCG_Games.Domain.Model.DTO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FCG_Games.Application.Service;

public class GameMessageService : IGameMessageService
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    private readonly ServiceBusClient _client;
    private readonly string _queueName = "games";

    public GameMessageService(IConfiguration config)
    {
        _config = config;
        _connectionString = this._config.GetValue<string>("AzureServiceBus:ConnectionString");
        _client = new ServiceBusClient(_connectionString);
    }

    public async Task SendMessageAsync(GameResponse game)
    {
        var sender = _client.CreateSender(_queueName);

        var body = JsonSerializer.Serialize(game);

        var message = new ServiceBusMessage(body)
        {
            ContentType = "application/json",
            MessageId = game.Id.ToString()
        };

        await sender.SendMessageAsync(message);
    }
}
