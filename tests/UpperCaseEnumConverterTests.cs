using System.Text.Json;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Tests;

public enum TestEnum
{
    FirstValue,
    SecondValue,
    ThirdValue
}

public class UpperCaseEnumConverterTests
{
    private readonly JsonSerializerOptions _serializerOptions;

    public UpperCaseEnumConverterTests()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            Converters = { new UpperCaseEnumConverter<TestEnum>() },
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public void Read_ShouldDeserializeSingleEnumValue()
    {
        const string json = "\"FIRSTVALUE\"";

        var result = JsonSerializer.Deserialize<TestEnum>(json, _serializerOptions);

        Assert.Equal(TestEnum.FirstValue, result);
    }

    [Fact]
    public void Read_ShouldDeserializeEnumArray()
    {
        const string json = "[\"FIRSTVALUE\", \"SECONDVALUE\", \"THIRDVALUE\"]";

        var result = JsonSerializer.Deserialize<TestEnum[]>(json, _serializerOptions);

        Assert.Equal([TestEnum.FirstValue, TestEnum.SecondValue, TestEnum.ThirdValue], result);
    }

    [Fact]
    public void Read_ShouldThrowJsonException_ForInvalidEnumValue()
    {
        const string json = "\"INVALIDVALUE\"";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestEnum>(json, _serializerOptions));
    }

    [Fact]
    public void Write_ShouldSerializeSingleEnumValueToUppercase()
    {
        const TestEnum value = TestEnum.FirstValue;

        var json = JsonSerializer.Serialize(value, _serializerOptions);

        Assert.Equal("\"FIRSTVALUE\"", json);
    }

    [Fact]
    public void Write_ShouldSerializeEnumArrayToUppercase()
    {
        var values = new[] { TestEnum.FirstValue, TestEnum.SecondValue, TestEnum.ThirdValue };

        var json = JsonSerializer.Serialize(values, _serializerOptions);

        Assert.Equal("[\"FIRSTVALUE\",\"SECONDVALUE\",\"THIRDVALUE\"]", json);
    }

    [Fact]
    public void Read_ShouldThrowJsonException_ForNullJson()
    {
        const string json = "null";

        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestEnum>(json, _serializerOptions));
    }

    [Fact]
    public void Write_ShouldHandleEmptyEnumArray()
    {
        var values = Array.Empty<TestEnum>();

        var json = JsonSerializer.Serialize(values, _serializerOptions);

        Assert.Equal("[]", json);
    }
}
