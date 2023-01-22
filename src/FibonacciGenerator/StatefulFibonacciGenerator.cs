using System.Collections.Concurrent;
using System.Numerics;
using Abstractions.Fibonacci;

namespace FibonacciGenerator;

public class StatefulFibonacciGenerator : IFibonacciGenerator<FibonacciValueDto>
{
    /// <summary>
    /// Тут я сделал допущение, что у нас больмень надежный канал связи и нет никакого MITM, способного вклиниться в процесс общения
    /// сервисов и подменить на рандомное число Value. По сему "кэш" содержит текущее значение,
    /// которое одновременно является предыдущим значением для стороннего сервиса. Значит формула рассчета N(i) числа такова:
    /// N(i - 1) - берется из данного кеша, а N(i) приходит в запросе как value.Value.
    /// Ключем является Correlation Id.
    /// </summary>
    private readonly ConcurrentDictionary<Guid, BigInteger> _currentValueCache = new ();

    public FibonacciValueDto GenerateNext(FibonacciValueDto value)
    {
        if (value.Value == 0 || value.Value == 1)
        {
            if (!_currentValueCache.TryGetValue(value.CorrelationId, out var startSeq))
            {
                _currentValueCache.AddOrUpdate(value.CorrelationId, 1, (k, v) => 1);
                return value with { Value = 1 };
            }

            var next = startSeq + value.Value;
            _currentValueCache.AddOrUpdate(value.CorrelationId, next, (k, v) => next);
            return value with { Value = next };
        }

        if (_currentValueCache.TryGetValue(value.CorrelationId, out var prev))
        {
            var next = prev + value.Value;
            _currentValueCache.AddOrUpdate(value.CorrelationId, next, (k, v) => next);
            return value with { Value = next };
        }

        throw new InvalidOperationException($"Not a fib sequence number ( {value.Value} ) for requests with correlation Id {value.CorrelationId}");
    }
}