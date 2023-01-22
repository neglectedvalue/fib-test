using Abstractions.Fibonacci;

namespace FibonacciGenerator;

public class StatelessFibonacciGenerator : IFibonacciGenerator<StatelessFibonacciValueDto>
{
    public StatelessFibonacciValueDto GenerateNext(StatelessFibonacciValueDto value)
        => value with { PrevValue = value.Value, Value = value.Value + value.PrevValue};
}