using Configuration;
using EasyNetQ;
using MessageSender.RabbitMq.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageSender.RabbitMq;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rmqSettings = configuration.GetSettings<RabbitMqSettings, RabbitMqSettingsValidator>();
        serviceCollection.RegisterEasyNetQ(BuildRmqConnectionString(rmqSettings),
            act => act.EnableSystemTextJson());
        return serviceCollection;
    }

    private static string BuildRmqConnectionString(this RabbitMqSettings settings)
        => $"host={settings.Host}:{settings.Port};username={settings.User};password={settings.Password}";
}