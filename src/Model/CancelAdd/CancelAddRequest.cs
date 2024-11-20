using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.CancelAdd;

public class CancelAddRequest
{
    public string OrgIdRef { get; private set; }

    public CancelAddRequest(string orgIdRef)
    {
        OrgIdRef = orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
    }
}