using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public class GetAllRequest : FrejaApiRequest<GetAllResponse>
{
    [JsonIgnore]
    public override string Endpoint => "users/getAll";

    [JsonIgnore]
    public override string? Action  => null;
}
