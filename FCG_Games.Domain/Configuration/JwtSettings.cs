namespace FCG_Games.Domain.Configuration;

public class JwtSettings
{
    public string SecretKey { get; set; } = default!;

    public int ExpirationInMinutes { get; set; } = 60;
}
