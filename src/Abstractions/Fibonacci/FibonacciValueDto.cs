using System.Numerics;
using JetBrains.Annotations;

namespace Abstractions.Fibonacci;

[PublicAPI]
public record FibonacciValueDto
{
    public BigInteger Value { get; init; }

    public BigInteger PrevValue { get; init; }
    public Guid CorrelationId { get; init; }
}