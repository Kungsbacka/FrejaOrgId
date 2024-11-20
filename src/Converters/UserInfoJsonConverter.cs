using FrejaOrgId.Model.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Converters
{
    /// <summary>
    /// A custom JSON converter for serializing and deserializing objects of type UserInfoBase.
    ///
    /// This JsonConverter is asymetric. The JSON read will not look the same as the JSON written. This is intentional.
    /// </summary>
    internal class UserInfoJsonConverter : JsonConverter<UserInfoBase>
    {
        private const string LeftBracket = "{";
        private const string RightBracket = "}";

        /// <summary>
        /// Reads and converts the JSON to the specified type. If the JSON token is not a string, it throws a JsonException.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type to convert to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns>
        /// A UserInfoBase object if the JSON is valid; otherwise, null.
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

            if (IsJson(value))
            {
                return JsonSerializer.Deserialize<SsnUserInfo>(value, options);
            }

            return new StringUserInfo(value);
        }

        /// <summary>
        /// Writes a UserInfoBase object to the provided Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The UserInfoBase object to write.</param>
        /// <param name="options">The JsonSerializerOptions to use for serialization.</param>
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

        /// <summary>
        /// Determines whether the specified string is a JSON object or array by checking if it starts with '{' or '[' and ends with '}' or ']'.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>
        /// True if the string is a JSON object or array; otherwise, false.
        /// </returns>
        private static bool IsJson(string value)
        {
            return value.StartsWith(LeftBracket) && value.EndsWith(RightBracket);
        }
    }
}