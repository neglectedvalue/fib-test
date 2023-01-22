using Abstractions.Fibonacci;
using Abstractions.MessageSender;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace MessageSender.RabbitMq;

public class RabbitMqMessageSender : IMessageSender
{
    private readonly IBus _bus;
    private readonly ILogger<RabbitMqMessageSender> _logger;

    public RabbitMqMessageSender(IBus bus, ILogger<RabbitMqMessageSender> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public Task SendMessageAsync<TMessage>(TMessage message, CancellationToken token)
        where TMessage : FibonacciValueDto
    {
        _logger.LogInformation("Sending fibonacci message {Message}", message);

        try
        {
            return _bus.PubSub.PublishAsync(message, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}