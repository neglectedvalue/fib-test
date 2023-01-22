using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Abstractions.Fibonacci;
using Abstractions.MessageSender;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageSender.HttpClient;

public class HttpClientSender : IMessageSender
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly HttpClientSettings _httpClientSettings;
    private readonly ILogger<HttpClientSender> _logger;

    public HttpClientSender(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<HttpClientSettings> settings,
        ILogger<HttpClientSender> logger)
    {
        _logger = logger;
        _httpClientSettings = settings.CurrentValue;
        _httpClient = httpClientFactory.CreateClient(settings.CurrentValue.HttpClientName);
    }

    public async Task SendMessageAsync<TMessage>(TMessage message, CancellationToken token)
        where TMessage : FibonacciValueDto
    {
        _logger.LogInformation("Sending fibonacci message {Message}", message);

        try
        {
            var reqHttpContent = JsonContent.Create(message);
            var response =
                await _httpClient.PostAsync(_httpClientSettings.MethodUrl, reqHttpContent, token);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error during sending message {MessageId}", message.CorrelationId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}