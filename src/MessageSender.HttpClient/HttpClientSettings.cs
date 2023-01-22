using JetBrains.Annotations;

namespace MessageSender.HttpClient;

[PublicAPI]
public record HttpClientSettings
{
    public string HttpClientName { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public string MethodUrl { get; init; } = string.Empty;
}