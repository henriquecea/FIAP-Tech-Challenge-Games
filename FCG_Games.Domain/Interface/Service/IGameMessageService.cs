using FCG_Games.Domain.Model.DTO;

namespace FCG_Games.Domain.Interface.Service;

public interface IGameMessageService
{
    Task SendMessageAsync(GameResponse game);
}

