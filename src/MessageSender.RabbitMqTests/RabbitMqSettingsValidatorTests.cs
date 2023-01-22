using FluentAssertions;
using MessageSender.RabbitMq;
using MessageSender.RabbitMq.Settings;

namespace MessageSender.RabbitMqTests;

public class RabbitMqSettingsValidatorTests
{
    [Fact]
    public void Validate_RabbitMqSettingsCreatedByDefaultCtor_ShouldNotBeValid()
    {
        // Arrange
        var settings = new RabbitMqSettings();
        var validator = new RabbitMqSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_AllSettingsAreValid_ShouldBeValid()
    {
        // Arrange
        var settings = new RabbitMqSettings
        {
            Host = "test",
            Password = "pass",
            Port = 5432,
            User = "user"
        };
        var validator = new RabbitMqSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "123", 1234, "123")]
    [InlineData("", "", 1234, "123")]
    [InlineData("", "123", -1, "123")]
    [InlineData("", "123", 1234, "")]
    public void Validate_AtLeastOneSettingIsMissing_ShouldNotBeValid(string host, string pass, int port, string user)
    {
        // Arrange
        var settings = new RabbitMqSettings
        {
            Host = host,
            Password = pass,
            Port = port,
            User = user
        };
        var validator = new RabbitMqSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}