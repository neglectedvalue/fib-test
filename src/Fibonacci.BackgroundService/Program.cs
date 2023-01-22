using Abstractions.Fibonacci;
using Configuration;
using Fibonacci.BackgroundService;
using FibonacciGenerator;
using MessageSender.HttpClient;
using MessageSender.RabbitMq;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(config =>
    {
        config.AddAllSettingFiles();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddRabbitMq(ctx.Configuration);
        services.AddSingleton<IFibonacciGenerator<FibonacciValueDto>, StatefulFibonacciGenerator>();
        services.ConfigureAndValidateSettings<BackgroundServiceSettings, BackgroundServiceSettingsValidator>(ctx.Configuration);

        services.AddHttpClientSender(ctx.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();