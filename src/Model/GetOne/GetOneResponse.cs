using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetOne;

public class GetOneResponse : IGetOneResponse
{
    public string OrgIdRef { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
    public TransactionStatus Status { get; private set; }

    [JsonConverter(typeof(GetOneDetailsConverter))]
    public GetOneDetailsBase Details { get; private set; }

    [JsonConstructor]
    internal GetOneResponse(string orgIdRef, TransactionStatus status, GetOneDetailsBase details)
    {
        OrgIdRef = orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
        Status = status;
        Details = details.ThrowIfNull(nameof(details));
    }
}
