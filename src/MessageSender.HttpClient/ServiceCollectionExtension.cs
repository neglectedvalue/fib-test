using System.Net;
using System.Net.Sockets;
using Abstractions.MessageSender;
using Configuration;
using MessageSender.HttpClient.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace MessageSender.HttpClient;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHttpClientSender(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var settings = serviceCollection.ConfigureAndValidateSettings<HttpClientSettings, HttpClientSettingsValidator>(configuration);

        serviceCollection.AddHttpClient(settings.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(settings.Host);
        }).AddPolicyHandler(GetRetryPolicy(settings));

        serviceCollection.AddSingleton<IMessageSender, HttpClientSender>();
        return serviceCollection;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(HttpClientSettings settings)
    {
        var responseMessagePolicy = Policy
            .HandleResult<HttpResponseMessage>(ShouldRetry)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                    TimeSpan.FromMilliseconds(settings.RetrySettings.FirstDelayMs),
                    settings.RetrySettings.RetryCount));

        var exceptionPolicy = Policy
            .HandleInner<HttpRequestException>(ex => ex.InnerException is SocketException socketException)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(settings.RetrySettings.FirstDelayMs), settings.RetrySettings.RetryCount));

        var result = responseMessagePolicy.WrapAsync(exceptionPolicy);

        return result;

        bool ShouldRetry(HttpResponseMessage message)
        {
            if (message.RequestMessage == null)
            {
                return false;
            }

            var shouldRetryOnMethod = message.RequestMessage.Method == HttpMethod.Post;

            return shouldRetryOnMethod
                   && (message.StatusCode == HttpStatusCode.RequestTimeout
                       || ((int)message.StatusCode >= 500 && (int)message.StatusCode <= 599));
        }
    }
}