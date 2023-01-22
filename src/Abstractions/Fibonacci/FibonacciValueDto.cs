using System.Numerics;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Abstractions.Fibonacci;

[PublicAPI]
public record FibonacciValueDto
{
    [JsonConverter(typeof(BigIntegerJsonConverter))]
    public BigInteger Value { get; init; }

    public Guid CorrelationId { get; init; }
}

[PublicAPI]
public record StatelessFibonacciValueDto : FibonacciValueDto
{
    [JsonConverter(typeof(BigIntegerJsonConverter))]
    public BigInteger PrevValue { get; init; }
}