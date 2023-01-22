using Abstractions.Fibonacci;
using Abstractions.MessageSender;
using Microsoft.AspNetCore.Mvc;

namespace Fibonacci.WebApi.Controllers;

[ApiController]
[Route("[controller]/api")]
public class FibonacciController : ControllerBase
{
    private readonly ILogger<FibonacciController> _logger;
    private readonly IMessageSender _messageSender;
    private readonly IFibonacciGenerator<FibonacciValueDto> _fibonacciGenerator;

    public FibonacciController(ILogger<FibonacciController> logger,
        IMessageSender messageSender,
        IFibonacciGenerator<FibonacciValueDto> fibonacciGenerator)
    {
        _logger = logger;
        _messageSender = messageSender;
        _fibonacciGenerator = fibonacciGenerator;
    }

    [HttpPost]
    [Route("calculate")]
    public async Task<IActionResult> CalculateFibAsync([FromBody]FibonacciValueDto value)
    {
        _logger.LogInformation($@"{value}");
        var nextFib = _fibonacciGenerator.GenerateNext(value);
        await _messageSender.SendMessageAsync(nextFib);

        return Ok();
    }
}