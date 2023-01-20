namespace MessageSender.RabbitMq;

public record RabbitMqSettings
{
    public string Host { get; init; } = string.Empty;
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
};