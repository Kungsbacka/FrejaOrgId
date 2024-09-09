using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrejaOrgId.Model;

namespace FrejaOrgId.Converters
{
    /// <summary>
    /// A JSON converter for the UserInfoBase class that encodes and decodes the object to and from Base64 format.
    /// </summary>
    internal class UserInfoBase64Converter : JsonConverter<UserInfoBase>
    {
        /// <summary>
        /// Reads and converts the JSON to the specified UserInfoBase type. If the JSON token is not a string, it throws a JsonException.
        /// If the string is a valid Base64 encoded string, it deserializes it to SsnUserInfo; otherwise, it returns a StringUserInfo.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type to convert to.</param>
        /// <param name="options">Options to control the behavior during deserialization.</param>
        /// <returns>
        /// A UserInfoBase object which can be either SsnUserInfo or StringUserInfo, or null if the input string is null.
        /// </returns>
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
                Span<byte> buffer = Convert.FromBase64String(value).AsSpan();
                return JsonSerializer.Deserialize<SsnUserInfo>(buffer, options);
            }
            return new StringUserInfo(value);
        }

        /// <summary>
        /// Writes a UserInfoBase object to a Utf8JsonWriter. If the object is of type SsnUserInfo, it serializes it to a base64 string. 
        /// If the object is of type StringUserInfo, it writes the string value directly. Throws a JsonException for unsupported types.
        /// </summary>
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