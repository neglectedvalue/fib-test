using Configuration;
using FluentValidation;

namespace MessageSender.RabbitMq;

public class RabbitMqSettingsValidator : AbstractSettingsValidator<RabbitMqSettings>
{
    public RabbitMqSettingsValidator()
    {
        RuleFor(x => x.Host).NotEmpty().WithMessage($"{nameof(RabbitMqSettings.Host)} must not be empty.");
        RuleFor(x => x.Port).NotEmpty()
            .Must(p => p > 0).WithMessage($"{nameof(RabbitMqSettings.Port)} must not be empty.");
        RuleFor(x => x.User).NotEmpty().WithMessage($"{nameof(RabbitMqSettings.User)} must not be empty.");
        RuleFor(x => x.Password).NotEmpty().WithMessage($"{nameof(RabbitMqSettings.Password)} must not be empty.");
    }
}