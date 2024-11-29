using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.InitAdd;

public class InitAddResponse : IInitAddResponse
{
    public string OrgIdRef { get; private set; }

    [JsonConstructor]
    internal InitAddResponse(string orgIdRef)
    {
        OrgIdRef = orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
    }
}
