using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model;

public class UserInfo
{
    public UserOrganisationId OrganisationId { get; private set; }

    public UserSsn Ssn { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<UserRegistrationState>))]
    public UserRegistrationState RegistrationState { get; private set; }

    public UserInfo(UserOrganisationId organisationId, UserSsn ssn, UserRegistrationState registrationState)
    {
        ArgumentNullException.ThrowIfNull(organisationId);
        ArgumentNullException.ThrowIfNull(ssn);

        OrganisationId = organisationId;
        Ssn = ssn;
        RegistrationState = registrationState;
    }
}
