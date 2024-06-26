using FrejaOrgId.Model;
using System.Buffers.Text;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.JsonWebTokens;


namespace FrejaOrgId.Converters
{
    /// <summary>
    /// A custom JSON converter for the DetailsBase class, used to handle serialization and deserialization
    /// of objects derived from DetailsBase.
    /// 
    /// This JsonConverter is asymetric. The JSON read will not look the same as the JSON written.
    /// This is intentional.
    /// </summary>
    internal class GetOneDetailsConverter : JsonConverter<GetOneDetailsBase>
    {
        private const char Dot = '.';
        private const byte Padding = 61;
        private const byte Plus = 43;
        private const byte Minus = 45;
        private const byte Underscore = 95;
        private const byte ForwardSlash = 47;
        private static readonly JsonWebTokenHandler _tokenHandler = new();

        /// <summary>
        /// Reads and converts the JSON to the specified type. If the JSON token is not a string, it throws a JsonException.
        /// If the string can be read as a JWS token, it deserializes it to an ApprovedDetails object and sets the original JWS.
        /// Otherwise, it returns a new StringDetails object with the string value.
        /// </summary>
        /// <returns>
        /// A DetailsBase object which can be either an ApprovedDetails or StringDetails based on the input JSON string.
        /// </returns>
        public override GetOneDetailsBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            if (_tokenHandler.CanReadToken(value))
            {
                ApprovedGetOneDetails details = DeserializeJws(value, options);
                details.SetOriginalJws(value);
                return details;
            }
            return new StringGetOneDetails(value);
        }

        /// <summary>
        /// Writes a JSON representation of a DetailsBase object using Utf8JsonWriter.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The DetailsBase object to serialize.</param>
        /// <param name="options">Options to control the serialization behavior.</param>
        public override void Write(Utf8JsonWriter writer, GetOneDetailsBase value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case ApprovedGetOneDetails approvedDetails:
                    JsonSerializer.Serialize(writer, approvedDetails, options);
                    break;
                case StringGetOneDetails stringDetails:
                    writer.WriteStringValue(stringDetails.Value);
                    break;
                default:
                    throw new JsonException("Expected an object that inherits from DetailsBase");
            }
        }

        /// <summary>
        /// Deserializes a JSON Web Signature (JWS) string into an ApprovedDetails object using the provided JsonSerializerOptions.
        /// </summary>
        /// <param name="jws">The JWS string to be deserialized.</param>
        /// <param name="options">The JsonSerializerOptions to use during deserialization.</param>
        /// <returns>
        /// An ApprovedDetails object deserialized from the JWS payload.
        /// </returns>
        /// <exception cref="JsonException">Thrown when the JWS format is invalid or deserialization fails.</exception>
        private static ApprovedGetOneDetails DeserializeJws(string jws, JsonSerializerOptions options)
        {
            var jwsReadOnlySpan = jws.AsSpan();
            int firstDot = jwsReadOnlySpan.IndexOf(Dot);
            int length = jwsReadOnlySpan.LastIndexOf(Dot) - firstDot - 1;
            if (length <= 0)
            {
                throw new JsonException();
            }
            var payload = jwsReadOnlySpan.Slice(firstDot + 1, length);
            int padding = (4 - (payload.Length % 4)) % 4;
            Span<byte> payloadSpan = stackalloc byte[payload.Length + padding];
            Encoding.UTF8.GetBytes(payload, payloadSpan);
            for (int i = 0; i < padding; i++)
            {
                payloadSpan[payload.Length + i] = Padding;
            }
            payloadSpan.Replace(Minus, Plus);
            payloadSpan.Replace(Underscore, ForwardSlash);
            Base64.DecodeFromUtf8InPlace(payloadSpan, out int bytesWritten);
            ApprovedGetOneDetails? approvedDetails = JsonSerializer.Deserialize<ApprovedGetOneDetails>(payloadSpan[..bytesWritten], options);
            return approvedDetails ?? throw new JsonException();
        }
    }
}