using Configuration;
using FluentValidation;

namespace MessageSender.RabbitMq;

public class RabbitMqSettingsValidator : AbstractSettingsValidator<RabbitMqSettings>
{
    public RabbitMqSettingsValidator()
    {
        RuleFor(x => x.Host).NotEmpty();
        RuleFor(x => x.User).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}