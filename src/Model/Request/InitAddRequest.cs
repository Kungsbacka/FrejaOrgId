using FrejaOrgId.Model.Response;
using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public class InitAddRequest : FrejaApiRequest<InitAddResponse>
{
    [JsonIgnore]
    public override string Endpoint => "initAdd";

    [JsonIgnore]
    public override string? Action  => "initAddOrganisationIdRequest";

    [JsonConverter(typeof(UpperCaseEnumConverter<UserInfoType>))]
    public UserInfoType UserInfoType { get; private set; }

    [JsonConverter(typeof(UserInfoBase64Converter))]
    public UserInfoBase UserInfo { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<MinRegistrationLevel>))]
    public MinRegistrationLevel MinRegistrationLevel { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Expiry { get; private set; }

    public OrganisationId OrganisationId { get; private set; }

    public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId,
        MinRegistrationLevel minRegistrationLevel, int expirationInMinutes)
    {
        ArgumentNullException.ThrowIfNull(organisationId);
        ArgumentNullException.ThrowIfNull(userInfo);

        if (expirationInMinutes <= 0)
        {
            // When expiry is zero, it will not be serialized as part of the request.
            // Default value when expiry is not present in the request is 7 days.
            Expiry = 0;
        }
        else if (expirationInMinutes is < 2 or > 30 * 24 * 60)
        {
            throw new ArgumentException("Expiration time must be between 2 and 43200 minutes (30 days), or less than or equal to zero (0) for default expiration.", 
                nameof(expirationInMinutes));
        }
        else
        {
            Expiry = DateTimeOffset.Now.AddMinutes(expirationInMinutes).ToUnixTimeMilliseconds();
        }

        switch (userInfoType)
        {
            case UserInfoType.Ssn:
                {
                    if (userInfo is not SsnUserInfo)
                    {
                        throw new ArgumentException("If UserInfoType is 'SSN' UserInfo must be a SsnUserInfo object",
                            nameof(userInfo));
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
                        throw new ArgumentException($"If UserInfoType is '{UserInfoType}' UserInfo must be a string",
                            nameof(userInfo));
                    }

                    break;
                }
        }

        UserInfoType = userInfoType;
        UserInfo = userInfo;
        MinRegistrationLevel = minRegistrationLevel;
        OrganisationId = organisationId;
    }

    public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId, MinRegistrationLevel minRegistrationLevel)
        : this(userInfoType, userInfo, organisationId, minRegistrationLevel, expirationInMinutes: 0) { }

    public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId, int expirationInMinutes)
        : this(userInfoType, userInfo, organisationId, MinRegistrationLevel.Extended, expirationInMinutes) { }

    public InitAddRequest(UserInfoType userInfoType, UserInfoBase userInfo, OrganisationId organisationId)
        : this(userInfoType, userInfo, organisationId, MinRegistrationLevel.Extended, expirationInMinutes: 0) { }
}
