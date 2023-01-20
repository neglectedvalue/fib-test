using JetBrains.Annotations;

namespace Abstractions.Fibonacci;

[PublicAPI]
public interface IFibonacciGenerator
{
    FibonacciValueDto GenerateNext(FibonacciValueDto value);
}