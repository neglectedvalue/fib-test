using Abstractions.Fibonacci;
using JetBrains.Annotations;

namespace Abstractions.MessageSender;

[PublicAPI]
public interface IMessageSender
{
    Task SendMessageAsync<TMessage>(TMessage message, CancellationToken token = default)
        where TMessage : FibonacciValueDto;
}