using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageSender.RabbitMq;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var rmqSettings = configuration.GetSettings<RabbitMqSettings, RabbitMqSettingsValidator>();
        serviceCollection.RegisterEasyNetQ(BuildRmqConnectionString(rmqSettings));
        return serviceCollection;
    }

    private static string BuildRmqConnectionString(this RabbitMqSettings settings)
    {
        return $"host={settings.Host};username={settings.User};password={settings.Password}";
    }
}