using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Response;

public class GetOneResponse : FrejaApiResponse
{
    public string OrgIdRef { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
    public TransactionStatus Status { get; private set; }

    [JsonConverter(typeof(GetOneDetailsConverter))]
    public GetOneDetailsBase? Details { get; private set; }

    public GetOneResponse(string orgIdRef, TransactionStatus status, GetOneDetailsBase? details)
    {
        ArgumentException.ThrowIfNullOrEmpty(orgIdRef);

        OrgIdRef = orgIdRef;
        Status = status;
        Details = details;
    }
}
