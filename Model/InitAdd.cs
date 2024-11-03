using System.Text.Json.Serialization;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Model
{
    public record InitAddRequest
    {
        [JsonConverter(typeof(UpperCaseEnumConverter<UserInfoType>))]
        public UserInfoType UserInfoType { get; init; }

        [JsonConverter(typeof(UserInfoBase64Converter))]
        public UserInfoBase UserInfo { get; init; }

        [JsonConverter(typeof(UpperCaseEnumConverter<MinRegistrationLevel>))]
        public MinRegistrationLevel? MinRegistrationLevel { get; init; }

        public long? Expiry { get; init; }
        public OrganisationId OrganisationId { get; init; }

        public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId,
            MinRegistrationLevel? minRegistrationLevel, long? expiry)
        {
            switch (userInfoType)
            {
                case UserInfoType.Ssn:
                {
                    if (userInfo is not SsnUserInfo)
                    {
                        throw new InvalidOperationException(
                            "If UserInfoType is 'SSN' UserInfo must be a SsnUserInfo object");
                    }

                    break;
                }
                case UserInfoType.Inferred when userInfo is StringUserInfo stringUserInfo:
                {
                    if (stringUserInfo.Value != "N/A")
                    {
                        throw new ArgumentException("If UserInfoType is 'Inferred' UserInfo value must be 'N/A'",
                            nameof(userInfo));
                    }

                    break;
                }
                case UserInfoType.Inferred:
                    throw new ArgumentException(
                        "If UserInfoType is 'Inferred' UserInfo value must be of type StringUserInfo",
                        nameof(userInfo));
                default:
                {
                    if (userInfo is not StringUserInfo)
                    {
                        throw new InvalidOperationException(
                            $"If UserInfoType is '{UserInfoType}' UserInfo must be a string");
                    }

                    break;
                }
            }

            UserInfoType = userInfoType;
            UserInfo = userInfo;
            MinRegistrationLevel = minRegistrationLevel;
            Expiry = expiry;
            OrganisationId = organisationId;
        }
    }

    public record InitAddResponse(string OrgIdRef);

    public record OrganisationId(
        string Title,
        string IdentifierName,
        string Identifier,
        [property: JsonConverter(typeof(UpperCaseEnumConverter<IdentifierDisplayType[]>))]
        IdentifierDisplayType[]? IdentifierDisplayTypes,
        AdditionalAttribute[]? AdditionalAttributes);

    public enum IdentifierDisplayType
    {
        Text,
        QR_Code
    }
}