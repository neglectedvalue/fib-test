using Abstractions.MessageSender;
using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        });

        serviceCollection.AddSingleton<IMessageSender, HttpClientSender>();
        return serviceCollection;
    }
}