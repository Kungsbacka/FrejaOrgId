using FrejaOrgId.Model;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FrejaOrgId.Converters
{

    // This JsonConverter is asymetric. The JSON read will not look the same
    // as the JSON written. This is intentional.
    internal class UserInfoJsonConverter : JsonConverter<UserInfoBase>
    {
        private const string LeftBracket = "{";
        private const string RightBracket = "}";

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
            if (IsJson(value))
            {
                return JsonSerializer.Deserialize<SsnUserInfo>(value, options);
            }
            return new StringUserInfo(value);
        }

        public override void Write(Utf8JsonWriter writer, UserInfoBase value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case SsnUserInfo ssnUserinfo:
                    JsonSerializer.Serialize(writer, ssnUserinfo, options);
                    break;
                case StringUserInfo userInfo:
                    writer.WriteStringValue(userInfo.Value);
                    break;
                default:
                    throw new JsonException("Expected an object that inherits from UserInfoBase");
            }
        }

        private static bool IsJson(string value)
        {
            return value.StartsWith(LeftBracket) && value.EndsWith(RightBracket);
        }
    }
}
