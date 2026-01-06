namespace FCG_Games.Domain.Model;

public class GameModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Gender { get; set; }

    public decimal Value { get; set; }
}

