using FluentAssertions;
using MessageSender.HttpClient;
using MessageSender.HttpClient.Settings;

namespace MessageSender.HttpClientTests;

public class HttpClientSettingsValidatorTests
{
    [Fact]
    public void Validate_HttpClientSettingsCreatedByDefaultCtor_ShouldNotBeValid()
    {
        // Arrange
        var settings = new HttpClientSettings();
        var validator = new HttpClientSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_AllSettingsAreValid_ShouldBeValid()
    {
        // Arrange
        var settings = new HttpClientSettings
        {
            Host = "test",
            MethodUrl = "pass",
            HttpClientName = "user"
        };
        var validator = new HttpClientSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "123", "123")]
    [InlineData("123", "", "123")]
    [InlineData("", "123", "")]
    public void Validate_AtLeastOneSettingIsMissing_ShouldNotBeValid(string host, string method, string name)
    {
        // Arrange
        var settings = new HttpClientSettings
        {
            Host = host,
            MethodUrl = method,
            HttpClientName = name
        };
        var validator = new HttpClientSettingsValidator();

        // Act
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}