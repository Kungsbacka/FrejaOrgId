using System.Text;
using System.Text.Json;
using FrejaOrgId.Converters;
using FrejaOrgId.Model;

namespace FrejaOrgId.Tests;

public class UserInfoBase64ConverterTests
{
    private readonly JsonSerializerOptions _serializerOptions;

    public UserInfoBase64ConverterTests()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            Converters = { new UserInfoBase64Converter() },
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public void Read_ShouldDeserializeBase64ToSsnUserInfo()
    {
        var ssnUserInfo = new SsnUserInfo("SE", "198001010000");
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ssnUserInfo)));

        var result = JsonSerializer.Deserialize<UserInfoBase>($"\"{base64}\"", _serializerOptions);

        var ssnResult = Assert.IsType<SsnUserInfo>(result);
        Assert.Equal(ssnUserInfo.Country, ssnResult.Country);
        Assert.Equal(ssnUserInfo.Ssn, ssnResult.Ssn);
    }

    [Fact]
    public void Read_ShouldDeserializePlainStringToStringUserInfo()
    {
        const string plainString = "plain string";

        var result = JsonSerializer.Deserialize<UserInfoBase>($"\"{plainString}\"", _serializerOptions);

        var stringResult = Assert.IsType<StringUserInfo>(result);
        Assert.Equal(plainString, stringResult.Value);
    }

    [Fact]
    public void Write_ShouldSerializeSsnUserInfoToBase64()
    {
        var ssnUserInfo = new SsnUserInfo("SE", "198001010000");

        string json = JsonSerializer.Serialize<UserInfoBase>(ssnUserInfo, _serializerOptions);

        var decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(json.Trim('"')));
        var deserialized = JsonSerializer.Deserialize<SsnUserInfo>(decodedJson);

        Assert.NotNull(deserialized);
        Assert.Equal(ssnUserInfo.Country, deserialized!.Country);
        Assert.Equal(ssnUserInfo.Ssn, deserialized.Ssn);
    }

    [Fact]
    public void Write_ShouldSerializeStringUserInfoToPlainString()
    {
        const string plainString = "plain string";
        var stringUserInfo = new StringUserInfo(plainString);

        string json = JsonSerializer.Serialize<UserInfoBase>(stringUserInfo, _serializerOptions);

        Assert.Equal($"\"{plainString}\"", json);
    }

    [Fact]
    public void Read_ShouldThrowJsonExceptionForInvalidJsonToken()
    {
        const string invalidJson = "123";

        // Act & Assert
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