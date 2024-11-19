using System.Text;
using System.Text.Json;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Tests;

public class UnixTimeConverterTests
{
    private readonly UnixTimeConverter _converter = new();
    private readonly JsonSerializerOptions _serializerOptions;

    private readonly long _knownUnixTimeMilliseconds = 1577882999088;
    private readonly DateTime _knownDateTime = new(year: 2020, month: 1, day: 1, hour: 12, minute: 49, second: 59, millisecond: 88, DateTimeKind.Utc);

    public UnixTimeConverterTests()
    {
        // Configure JsonSerializerOptions with the custom converter
        _serializerOptions = new JsonSerializerOptions
        {
            Converters = { _converter },
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public void Read_ShouldConvertUnixMillisecondsToDateTime()
    {
        var json = _knownUnixTimeMilliseconds.ToString();
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();

        DateTime result = _converter.Read(ref reader, typeof(DateTime), _serializerOptions);
        
        Assert.Equal(_knownDateTime, result);
    }

    [Fact]
    public void Write_ShouldConvertDateTimeToUnixMilliseconds()
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        _converter.Write(writer, _knownDateTime, _serializerOptions);
        writer.Flush();

        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal(_knownUnixTimeMilliseconds.ToString(), json);
    }

    [Fact]
    public void JsonSerializer_ShouldUseUnixTimeConverterForDeserialization()
    {
        string json = _knownUnixTimeMilliseconds.ToString();

        DateTime result = JsonSerializer.Deserialize<DateTime>(json, _serializerOptions);

        Assert.Equal(_knownDateTime, result);
    }

    [Fact]
    public void JsonSerializer_ShouldUseUnixTimeConverterForSerialization()
    {
        string json = JsonSerializer.Serialize(_knownDateTime, _serializerOptions);

        Assert.Equal(_knownUnixTimeMilliseconds.ToString(), json);
    }
}