using JetBrains.Annotations;

namespace MessageSender.HttpClient.Settings;

[PublicAPI]
public record RetryPolicySettings
{
    public int RetryCount { get; init; } = 3;

    public int FirstDelayMs { get; init; } = 1000;
}