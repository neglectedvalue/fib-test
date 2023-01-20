using Abstractions.Fibonacci;
using FibonacciGenerator;
using FluentAssertions;

namespace FibonacciGenTests;

public class FibonacciGeneratorTests
{
    private readonly IFibonacciGenerator _fibonacciGenerator = new FibonacciGenerator.StatelessFibonacciGenerator();

    [Fact]
    public void FibonacciGenerator_GenerateNext_ShouldBeValid()
    {
        FibonacciValueDto start = new FibonacciValueDto { PrevValue = 0, Value = 1, CorrelationId = Guid.NewGuid() };
        var first = _fibonacciGenerator.GenerateNext(start);
        first.Value.Should().Be(1);

        var second = _fibonacciGenerator.GenerateNext(first);
        second.Value.Should().Be(2);

        var third = _fibonacciGenerator.GenerateNext(second);
        third.Value.Should().Be(3);

        var fourth = _fibonacciGenerator.GenerateNext(third);
        fourth.Value.Should().Be(5);
    }

    [Fact]
    public async Task FibonacciGenerator_GenerateNext_ShouldBeValidForTwoThreadsAsync()
    {
        Task[] tasks = { Task.Run(Seed), Task.Run(Seed) };

        await Task.WhenAll(tasks);
    }

    private void Seed()
    {
        FibonacciValueDto start = new FibonacciValueDto { PrevValue = 0, Value = 1, CorrelationId = Guid.NewGuid() };
        var first = _fibonacciGenerator.GenerateNext(start);
        first.Value.Should().Be(1);

        var second = _fibonacciGenerator.GenerateNext(first);
        second.Value.Should().Be(2);

        var third = _fibonacciGenerator.GenerateNext(second);
        third.Value.Should().Be(3);

        var fourth = _fibonacciGenerator.GenerateNext(third);
        fourth.Value.Should().Be(5);
    }
}