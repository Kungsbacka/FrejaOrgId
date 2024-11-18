namespace FrejaOrgId.Tests
{
    using System.Text.Json;
    using Xunit;
    using FrejaOrgId.Converters;
    using FrejaOrgId.Model;

    public class GetOneDetailsConverterTests
    {
        private readonly GetOneDetailsConverter _converter = new();
        private readonly JsonSerializerOptions _serializerOptions;

        public GetOneDetailsConverterTests()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                Converters = { _converter },
                PropertyNameCaseInsensitive = true
            };
        }

        [Fact]
        public void Read_ShouldDeserializeStringGetOneDetails_WhenInputIsPlainString()
        {
            const string json = "\"plain string\"";

            var result = JsonSerializer.Deserialize<GetOneDetailsBase>(json, _serializerOptions);

            Assert.IsType<StringGetOneDetails>(result);
            Assert.Equal("plain string", ((StringGetOneDetails)result).Value);
        }

        [Fact]
        public void Read_ShouldDeserializeApprovedGetOneDetails_WhenInputIsValidJws()
        {
            var json = $"\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJvcmdJZFJlZiI6IlRyTEE5emR4Q0JsTk9RTnZrZGhBTTE0bUptbEwyMGRpZ0M3K1FnRVZSd21FN1NIOFFtMHN3V0ljNndoZkttNFkiLCJzdGF0dXMiOiJBUFBST1ZFRCIsInVzZXJJbmZvVHlwZSI6IlNTTiIsInVzZXJJbmZvIjoiZXlKamIzVnVkSEo1SWpvaVUwVWlMQ0p6YzI0aU9pSXhPVGd3TURFd01UQXdNREFpZlE9PSIsIm1pblJlZ2lzdHJhdGlvbkxldmVsIjoiUExVUyIsInRpbWVzdGFtcCI6MTU3NzgzMzIwMDAwMCwic2lnbmF0dXJlVHlwZSI6IlNJTVBMRSIsInNpZ25hdHVyZURhdGEiOnsidXNlclNpZ25hdHVyZSI6IlRoZSBzaWduYXR1cmUgcHJvZHVjZWQgYnkgdGhlIGVuZCB1c2VyIiwiY2VydGlmaWNhdGVTdGF0dXMiOiJFdmlkZW5jZSBvZiBlbmQtdXNlcnMgY2VydGlmaWNhdGUgc3RhdHVzIn19.UvHMBJ4xqg8bNu1cv3_O_aNeZxLrDeqgV_ulCgakaYU\"";

            var result = JsonSerializer.Deserialize<GetOneDetailsBase>(json, _serializerOptions);

            var approvedResult = Assert.IsType<ApprovedGetOneDetails>(result);
            Assert.Equal(TransactionStatus.Approved, approvedResult.Status);
        }

        [Fact]
        public void Read_ShouldThrowJsonException_WhenInputIsInvalidJson()
        {
            const string invalidJson = "{\"invalid\":}";

            Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<GetOneDetailsBase>(invalidJson, _serializerOptions));
        }

        [Fact]
        public void Write_ShouldSerializeStringGetOneDetails()
        {
            // Arrange
            var value = new StringGetOneDetails("simple string");

            // Act
            var json = JsonSerializer.Serialize<GetOneDetailsBase>(value, _serializerOptions);

            // Assert
            Assert.Equal("\"simple string\"", json);
        }

        [Fact]
        public void Write_ShouldSerializeApprovedGetOneDetails()
        {
            // Arrange
            var value = new ApprovedGetOneDetails(
                 orgIdRef: "TrLA9zdxCBlNOQNvkdhAM14mJmlL20digC7+QgEVRwmE7SH8Qm0swWIc6whfKm4Y",
                 status: TransactionStatus.Approved,
                 userInfoType: UserInfoType.Ssn,
                 userInfo: new SsnUserInfo("SE", "198001010000"),
                 minRegistrationLevel: MinRegistrationLevel.Plus,
                 timestamp: DateTime.Parse("2020-01-01"),
                 signatureType: SignatureType.Simple,
                 signatureData: new SignatureData(
                     UserSignature: "The signature produced by the end user", 
                     CertificateStatus: "Evidence of end-users certificate status"
                  )
            );
            value.SetOriginalJws("Original JWS");

            var json = JsonSerializer.Serialize<GetOneDetailsBase>(value, _serializerOptions);

            // Assert
            var expectedJson = "{\"OrgIdRef\":\"TrLA9zdxCBlNOQNvkdhAM14mJmlL20digC7\\u002BQgEVRwmE7SH8Qm0swWIc6whfKm4Y\",\"Status\":\"APPROVED\",\"UserInfoType\":\"SSN\",\"UserInfo\":{\"Country\":\"SE\",\"Ssn\":\"198001010000\"},\"MinRegistrationLevel\":\"PLUS\",\"Timestamp\":1577833200000,\"SignatureType\":\"SIMPLE\",\"SignatureData\":{\"UserSignature\":\"The signature produced by the end user\",\"CertificateStatus\":\"Evidence of end-users certificate status\"},\"OriginalJws\":\"Original JWS\"}";
            Assert.Equal(expectedJson, json);
        }

        [Fact]
        public void Write_ShouldThrowJsonException_ForUnknownType()
        {
            var unknownValue = new UnknownGetOneDetails();

            Assert.Throws<JsonException>(() =>
                JsonSerializer.Serialize<GetOneDetailsBase>(unknownValue, _serializerOptions));
        }
    }

    // Test support classes
    internal record UnknownGetOneDetails : GetOneDetailsBase { }
}