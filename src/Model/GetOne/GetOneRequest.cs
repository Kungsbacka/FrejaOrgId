namespace FrejaOrgId.Model.GetOne;

public class GetOneRequest : IGetOneRequest
{
    public string OrgIdRef { get; private set; }

    public GetOneRequest(string orgIdRef)
    {
        OrgIdRef = orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
    }
}
