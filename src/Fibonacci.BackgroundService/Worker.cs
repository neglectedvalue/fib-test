using Abstractions.Fibonacci;
using Abstractions.MessageSender;
using EasyNetQ;
using Microsoft.Extensions.Options;

namespace Fibonacci.BackgroundService;

public class Worker : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFibonacciGenerator<FibonacciValueDto> _fibonacciGenerator;
    private readonly IMessageSender _messageSender;
    private readonly IBus _bus;
    private readonly BackgroundServiceSettings _backgroundServiceSettings;

    public Worker(ILogger<Worker> logger, IBus bus,
        IFibonacciGenerator<FibonacciValueDto> fibonacciGenerator,
        IMessageSender messageSender,
        IOptionsMonitor<BackgroundServiceSettings> backgroundSettings)
    {
        _bus = bus;
        _fibonacciGenerator = fibonacciGenerator;
        _messageSender = messageSender;
        _logger = logger;
        _backgroundServiceSettings = backgroundSettings.CurrentValue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var taskFactory = new TaskFactory(stoppingToken);

        await _bus.PubSub.SubscribeAsync<FibonacciValueDto>("fibonacciSubscription_" + Guid.NewGuid(),
            msg => taskFactory.StartNew( () => ProcessMessageAsync(msg, stoppingToken), stoppingToken),
            config => config.WithAutoDelete());

        var initialTasks = Enumerable.Range(0, _backgroundServiceSettings.AsyncRunsNumber)
            .Select(i => CreateSenderSeedTaskAsync(stoppingToken));

        await Task.WhenAll(initialTasks);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private Task CreateSenderSeedTaskAsync(CancellationToken stoppingToken)
        => _messageSender.SendMessageAsync(new FibonacciValueDto { Value = 0, CorrelationId = Guid.NewGuid() }, stoppingToken);

    private Task ProcessMessageAsync(FibonacciValueDto msg, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{msg}");
        var next = _fibonacciGenerator.GenerateNext(msg);
        return _messageSender.SendMessageAsync(next, stoppingToken);
    }
}