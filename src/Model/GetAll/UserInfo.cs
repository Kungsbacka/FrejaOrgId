using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetAll;

public class UserInfo
{
    public UserOrganisationId OrganisationId { get; private set; }

    public UserSsn Ssn { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<UserRegistrationState>))]
    public UserRegistrationState RegistrationState { get; private set; }

    [JsonConstructor]
    internal UserInfo(UserOrganisationId organisationId, UserSsn ssn, UserRegistrationState registrationState)
    {
        OrganisationId = organisationId.ThrowIfNull(nameof(organisationId));
        Ssn = ssn.ThrowIfNull(nameof(ssn));
        RegistrationState = registrationState;
    }
}
