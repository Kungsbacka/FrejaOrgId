using FrejaOrgId.Model.Shared;

namespace FrejaOrgId.Model.InitAdd;

public interface IInitAddRequest
{
    long Expiry { get; }
    MinRegistrationLevel MinRegistrationLevel { get; }
    OrganisationId OrganisationId { get; }
    UserInfoBase UserInfo { get; }
    UserInfoType UserInfoType { get; }
}
