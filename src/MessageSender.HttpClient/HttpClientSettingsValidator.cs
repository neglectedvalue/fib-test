using Configuration;
using FluentValidation;

namespace MessageSender.HttpClient;

public class HttpClientSettingsValidator : AbstractSettingsValidator<HttpClientSettings>
{
    public HttpClientSettingsValidator()
    {
        RuleFor(x => x.Host).NotEmpty().WithMessage($"{nameof(HttpClientSettings.Host)} must not be empty.");
        RuleFor(x => x.HttpClientName).NotEmpty().WithMessage($"{nameof(HttpClientSettings.HttpClientName)} must not be empty.");
        RuleFor(x => x.MethodUrl).NotEmpty().WithMessage($"{nameof(HttpClientSettings.MethodUrl)} must not be empty.");
    }
}