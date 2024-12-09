using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public class DeleteRequest : FrejaApiRequest<DeleteResponse>
{
    [JsonIgnore]
    public override string Endpoint => "delete";

    [JsonIgnore]
    public override string? Action  => "deleteOrganisationIdRequest";

    public string Identifier { get; private set; }

    public DeleteRequest(string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);

        Identifier = identifier;
    }
}