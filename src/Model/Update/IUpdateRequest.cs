using FrejaOrgId.Model.Shared;

namespace FrejaOrgId.Model.Update;

public interface IUpdateRequest
{
    AdditionalAttribute[] AdditionalAttributes { get; }
    string Identifier { get; }
}
