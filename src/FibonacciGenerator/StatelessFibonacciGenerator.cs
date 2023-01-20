using Abstractions.Fibonacci;

namespace FibonacciGenerator;

public class StatelessFibonacciGenerator : IFibonacciGenerator
{
    public FibonacciValueDto GenerateNext(FibonacciValueDto value)
        => value with { PrevValue = value.Value, Value = value.Value + value.PrevValue};
}