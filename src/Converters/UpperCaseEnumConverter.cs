using System.Collections;
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
            if (typeToConvert.IsArray)
            {
                Type? elementType = typeToConvert.GetElementType() ??
                                    throw new JsonException("Unable to determine array element type");
                ArrayList list = [];
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    if (Enum.TryParse(elementType, reader.GetString(), ignoreCase: true, out object? enumValue))
                    {
                        list.Add(enumValue);
                    }
                    else
                    {
                        throw new JsonException("Invalid enum value in array");
                    }
                }

                Array array = Array.CreateInstance(typeToConvert, list.Count);
                list.CopyTo(array);
                return (T)Convert.ChangeType(array, typeof(T));
            }
            else
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
        }

        /// <summary>
        /// Writes the specified value as a JSON string using the provided Utf8JsonWriter. If the value is an array,
        /// each element is written as an uppercase string in a JSON array. If the value is not an array, it is written
        /// as an uppercase string. Throws a JsonException if any enum value is null.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value is Array enumArray)
            {
                writer.WriteStartArray();
                foreach (object enumValue in enumArray)
                {
                    string stringValue = enumValue?.ToString() ?? throw new JsonException("Enum value cannot be null");
                    writer.WriteStringValue(stringValue.ToUpperInvariant());
                }

                writer.WriteEndArray();
            }
            else
            {
                string stringValue = value?.ToString() ?? throw new JsonException("Enum value cannot be null");
                writer.WriteStringValue(stringValue.ToUpperInvariant());
            }
        }
    }
}