using JetBrains.Annotations;

namespace MessageSender.RabbitMq.Settings;

[PublicAPI]
public record RabbitMqSettings
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
};