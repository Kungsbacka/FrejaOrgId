using System.Text.Json.Serialization;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Model
{
    public record GetAllRequest();

    public record GetAllResponse(
        UserInfo[] UserInfos
    );

    public record UserInfo(
        UserOrganisationId OrganisationId,
        UserSsn Ssn,
        [property: JsonConverter(typeof(UpperCaseEnumConverter<UserRegistrationState>))]
        UserRegistrationState RegistrationState);


    public record UserOrganisationId(string Title, string IdentifierName, string Identifier);

    public record UserSsn(string Ssn, string Country);

    public enum UserRegistrationState { Extended, Vetting_confirmed, Plus }
}
