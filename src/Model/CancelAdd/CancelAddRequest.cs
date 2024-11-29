namespace FrejaOrgId.Model.CancelAdd;

public class CancelAddRequest : ICancelAddRequest
{
    public string OrgIdRef { get; private set; }

    public CancelAddRequest(string orgIdRef)
    {
        OrgIdRef = orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
    }
}