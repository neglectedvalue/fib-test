using Configuration;
using FluentValidation;

namespace Fibonacci.BackgroundService;

public class BackgroundServiceSettingsValidator : AbstractSettingsValidator<BackgroundServiceSettings>
{
    public BackgroundServiceSettingsValidator()
    {
        RuleFor(x => x.AsyncRunsNumber).NotEmpty()
            .Must(p => p > 0)
            .WithMessage($"{nameof(BackgroundServiceSettings.AsyncRunsNumber)} should not be empty or less then 1");
    }
}