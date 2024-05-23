using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Converters
{
    /// <summary>
    /// A custom JSON converter for handling Unix time (seconds since epoch) to DateTime conversion.
    /// </summary>
    internal class UnixTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Reads a Unix timestamp in milliseconds from the JSON reader and converts it to a DateTime object.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read the Unix timestamp from.</param>
        /// <param name="typeToConvert">The type to convert (not used in this implementation).</param>
        /// <param name="options">Options for JSON serialization (not used in this implementation).</param>
        /// <returns>
        /// A DateTime object representing the Unix timestamp.
        /// </returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long value = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime;
        }

        /// <summary>
        /// Writes a DateTime value as a Unix time in milliseconds to the provided Utf8JsonWriter.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((new DateTimeOffset(value)).ToUnixTimeMilliseconds());
        }
    }
}