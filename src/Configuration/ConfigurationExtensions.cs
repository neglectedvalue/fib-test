using Microsoft.Extensions.Configuration;

namespace Configuration;

public static class ConfigurationExtensions
{
    public static TSetting GetSettings<TSetting>(this IConfiguration configuration, string? configurationSectionName = null)
        where TSetting : class, new()
    {
        var sectionName = configurationSectionName ?? typeof(TSetting).Name;
        var section = configuration.GetSectionSettings(sectionName);
        var settingInstance = section.GetSettingInstance<TSetting>(sectionName);

        return settingInstance;
    }

    public static TSetting GetSettings<TSetting, TValidator>(
        this IConfiguration configuration, string? settingsSectionName = null)
        where TSetting : class, new()
        where TValidator : AbstractSettingsValidator<TSetting>, new()
    {
        var sectionName = settingsSectionName ?? typeof(TSetting).Name;
        var settingInstance = configuration.GetSettings<TSetting>(sectionName);

        SettingsValidationHelper.Validate<TSetting, TValidator>(settingInstance, sectionName);

        return settingInstance;
    }

    public static IConfigurationBuilder AddAllSettingFiles(this IConfigurationBuilder configurationBuilder)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        return configurationBuilder
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env}.json", true, true)
            .AddEnvironmentVariables();
    }

    private static TSetting GetSettingInstance<TSetting>(this IConfigurationSection? section, string sectionName)
        where TSetting : class, new()
    {
        var settingInstance = section?.Get<TSetting>() ?? new TSetting();

        return settingInstance;
    }

    private static IConfigurationSection? GetSectionSettings(this IConfiguration configuration, string sectionName)
    {
        var section = configuration.GetSection(sectionName);

        return section;
    }
}