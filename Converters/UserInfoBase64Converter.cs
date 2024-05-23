using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrejaOrgId.Model;

namespace FrejaOrgId.Converters
{
    internal class UserInfoBase64Converter : JsonConverter<UserInfoBase>
    {
        public override UserInfoBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a JSON string");
            }
            string? value = reader.GetString();
            if (value == null)
            {
                return null;
            }
            if (Base64.IsValid(value))
            {
                return JsonSerializer.Deserialize<SsnUserInfo>(Convert.FromBase64String(value), options);
            }
            return new StringUserInfo(value);
        }

        public override void Write(Utf8JsonWriter writer, UserInfoBase value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case SsnUserInfo ssnUserinfo:
                    string json = JsonSerializer.Serialize(ssnUserinfo, options);
                    writer.WriteBase64StringValue(Encoding.UTF8.GetBytes(json).AsSpan());
                    break;
                case StringUserInfo userInfo:
                    writer.WriteStringValue(userInfo.Value);
                    break;
                default:
                    throw new JsonException("Expected an object that inherits from UserInfoBase");
            }
        }
    }
}
