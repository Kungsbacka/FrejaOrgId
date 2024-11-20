using System.Text.Json;
using FrejaOrgId.Model;
using FrejaOrgId.Converters;
using FrejaOrgId.Model.Shared;

namespace FrejaOrgId.Tests;

public class UserInfoJsonConverterTests
{
    private readonly JsonSerializerOptions _serializerOptions;

    public UserInfoJsonConverterTests()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            Converters = { new UserInfoJsonConverter() },
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public void Read_ShouldDeserializeJsonStringToSsnUserInfo()
    {
        const string ssn = "198001010000";
        const string country = "SE";

        const string escapedJson = $"{{\\\"Country\\\":\\\"{country}\\\",\\\"Ssn\\\":\\\"{ssn}\\\"}}";

        var result = JsonSerializer.Deserialize<UserInfoBase>($"\"{escapedJson}\"", _serializerOptions);

        var ssnResult = Assert.IsType<SsnUserInfo>(result);
        Assert.Equal(country, ssnResult.Country);
        Assert.Equal(ssn, ssnResult.Ssn);
    }

    [Fact]
    public void Read_ShouldDeserializePlainStringToStringUserInfo()
    {
        // Arrange: Plain string
        const string plainString = "plain string";

        // Act
        var result = JsonSerializer.Deserialize<UserInfoBase>($"\"{plainString}\"", _serializerOptions);

        // Assert
        var stringResult = Assert.IsType<StringUserInfo>(result);
        Assert.Equal(plainString, stringResult.Value);
    }

    [Fact]
    public void Write_ShouldSerializeSsnUserInfoAsJson()
    {
        var ssnUserInfo = new SsnUserInfo("SE", "198001010000");

        string json = JsonSerializer.Serialize<UserInfoBase>(ssnUserInfo, _serializerOptions);

        var deserialized = JsonSerializer.Deserialize<SsnUserInfo>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(ssnUserInfo.Country, deserialized.Country);
        Assert.Equal(ssnUserInfo.Ssn, deserialized.Ssn);
    }

    [Fact]
    public void Write_ShouldSerializeStringUserInfoAsPlainString()
    {
        const string plainString = "plain string";
        var stringUserInfo = new StringUserInfo(plainString);

        string json = JsonSerializer.Serialize<UserInfoBase>(stringUserInfo, _serializerOptions);

        Assert.Equal($"\"{plainString}\"", json);
    }

    [Fact]
    public void Read_ShouldReturnNullForNullInput()
    {
        const string json = "null";

        var result = JsonSerializer.Deserialize<UserInfoBase>(json, _serializerOptions);

        Assert.Null(result);
    }

    [Fact]
    public void Read_ShouldThrowJsonExceptionForInvalidJsonToken()
    {
        const string invalidJson = "123";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<UserInfoBase>(invalidJson, _serializerOptions));
    }

    [Fact]
    public void Write_ShouldThrowJsonExceptionForUnsupportedType()
    {
        var unsupportedType = new UnsupportedUserInfo();

        Assert.Throws<JsonException>(() => JsonSerializer.Serialize<UserInfoBase>(unsupportedType, _serializerOptions));
    }

    private class UnsupportedUserInfo : UserInfoBase;
}
