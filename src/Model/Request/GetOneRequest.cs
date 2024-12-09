using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;


namespace FrejaOrgId.Model.Request;

public class GetOneRequest : FrejaApiRequest<GetOneResponse>
{
    [JsonIgnore]
    public override string Endpoint => "getOneResult";

    [JsonIgnore]
    public override string? Action  => "getOneOrganisationIdResultRequest";

    public string OrgIdRef { get; private set; }

    public GetOneRequest(string orgIdRef)
    {
        ArgumentException.ThrowIfNullOrEmpty(orgIdRef);

        OrgIdRef = orgIdRef;
    }
}
