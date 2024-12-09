using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public class CancelAddRequest : FrejaApiRequest<CancelAddResponse>
{
    [JsonIgnore]
    public override string Endpoint => "cancelAdd";

    [JsonIgnore]
    public override string? Action  => "cancelAddOrganisationIdRequest";

    public string OrgIdRef { get; private set; }

    public CancelAddRequest(string orgIdRef)
    {
        ArgumentException.ThrowIfNullOrEmpty(orgIdRef);

        OrgIdRef = orgIdRef;
    }
}