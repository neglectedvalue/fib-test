namespace Fibonacci.BackgroundService;

public record BackgroundServiceSettings
{
    public int AsyncRunsNumber { get; init; } = 1;
}