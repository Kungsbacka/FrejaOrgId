using System.Text.Json.Serialization;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Model
{
    public record InitAddRequest
    {
        [JsonConverter(typeof(UpperCaseEnumConverter<UserInfoType>))]
        public UserInfoType UserInfoType { get; }
        [JsonConverter(typeof(UserInfoBase64Converter))]
        public UserInfoBase UserInfo { get; }
        [JsonConverter(typeof(UpperCaseEnumConverter<MinRegistrationLevel>))]
        public MinRegistrationLevel? MinRegistrationLevel { get; }
        public long? Expiry { get; }
        public OrganisationId OrganisationId { get; }

        public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId, MinRegistrationLevel? minRegistrationLevel, long? expiry)
        {
            if (userInfoType == UserInfoType.Ssn)
            {
                if (userInfo is not SsnUserInfo)
                {
                    throw new InvalidOperationException("If UserInfoType is 'SSN' UserInfo must be a SsnUserInfo object");
                }
            }
            else if (userInfoType == UserInfoType.Inferred)
            {
                if (userInfo is StringUserInfo stringUserInfo)
                {
                    if (stringUserInfo.Value != "N/A")
                    {
                        throw new ArgumentException("If UserInfoType is 'Inferred' UserInfo value must be 'N/A'", nameof(userInfo));
                    }
                }
                else
                {
                    throw new ArgumentException("If UserInfoType is 'Inferred' UserInfo value must be of type StringUserInfo", nameof(userInfo));
                }
            }
            else
            {
                if (userInfo is not StringUserInfo)
                {
                    throw new InvalidOperationException($"If UserInfoType is '{UserInfoType}' UserInfo must be a string");
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
        string Title, string IdentifierName, string Identifier,
        [property: JsonConverter(typeof(UpperCaseEnumConverter<IdentifierDisplayType[]>))] IdentifierDisplayType[]? IdentifierDisplayTypes,
        AdditionalAttribute[]? AdditionalAttributes);

    public enum IdentifierDisplayType { Text, QR_Code }
}

