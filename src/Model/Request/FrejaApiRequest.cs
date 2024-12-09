using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public abstract class FrejaApiRequest<TResponse> where TResponse : FrejaApiResponse
{
    [JsonIgnore]
    public abstract string Endpoint { get; }

    [JsonIgnore]
    public abstract string? Action { get; }
}
