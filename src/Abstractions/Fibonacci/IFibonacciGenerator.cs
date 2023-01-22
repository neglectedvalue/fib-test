using JetBrains.Annotations;

namespace Abstractions.Fibonacci;

[PublicAPI]
public interface IFibonacciGenerator<TFib> where TFib : FibonacciValueDto
{
    TFib GenerateNext(TFib value);
}