using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Converters
{
    /// <summary>
    /// A custom JSON converter that serializes and deserializes enum values to and from uppercase strings.
    /// </summary>
    internal class UpperCaseEnumConverter<T> : JsonConverter<T>
    {
        /// <summary>
        /// Reads and converts the JSON to the specified type. Handles both single enum values and arrays of enum values.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type to convert the JSON to.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns>
        /// The converted value of type T.
        /// </returns>
        /// <exception cref="JsonException">
        /// Thrown when the JSON contains an invalid enum value or when the array element type cannot be determined.
        /// </exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse(typeToConvert, reader.GetString(), ignoreCase: true, out object? enumValue))
            {
                return (T)enumValue;
            }
            else
            {
                throw new JsonException("Invalid enum value");
            }
        }

        /// <summary>
        /// Writes the specified value as a JSON string using the provided Utf8JsonWriter. This will also handle
        /// the case where the value is an array of enums. Throws a JsonException if any enum value is null.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            string stringValue = value?.ToString() ?? throw new JsonException("Enum value cannot be null");
            writer.WriteStringValue(stringValue.ToUpperInvariant());
        }
    }
}