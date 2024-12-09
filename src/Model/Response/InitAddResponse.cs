namespace FrejaOrgId.Model.Response;

public class InitAddResponse : FrejaApiResponse
{
    public string OrgIdRef { get; private set; }

    public InitAddResponse(string orgIdRef)
    {
        ArgumentException.ThrowIfNullOrEmpty(orgIdRef);

        OrgIdRef = orgIdRef;
    }
}
