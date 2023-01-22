using Abstractions.Fibonacci;
using FibonacciGenerator;
using FluentAssertions;

namespace FibonacciGenTests;

public class StatefulFibonacciGeneratorTests
{
    private readonly IFibonacciGenerator<FibonacciValueDto> _fistSrv = new StatefulFibonacciGenerator();
    private readonly IFibonacciGenerator<FibonacciValueDto> _secondSrv = new StatefulFibonacciGenerator();

    [Fact]
    public void StatefulFibonacciGenerator_GenerateNext_ShouldBeValidForTwoGenerators()
    {
        var fib0 = new FibonacciValueDto { CorrelationId = Guid.NewGuid(), Value = 0 };
        var fib1 = _fistSrv.GenerateNext(fib0);
        fib1.Should().Be(1);

        var fib2 = _secondSrv.GenerateNext(fib1);
        fib2.Should().Be(1);

        var fib3 = _fistSrv.GenerateNext(fib2);
        fib3.Should().Be(2);

        var fib4 = _secondSrv.GenerateNext(fib3);
        fib4.Should().Be(3);

        var fib5 = _fistSrv.GenerateNext(fib4);
        fib5.Should().Be(5);
    }
}