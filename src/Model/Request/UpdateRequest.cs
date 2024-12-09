using FrejaOrgId.Model.Response;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Request;

public class UpdateRequest : FrejaApiRequest<UpdateResponse>
{
    [JsonIgnore]
    public override string Endpoint => "update";

    [JsonIgnore]
    public override string? Action  => "updateOrganisationIdRequest";

    public string Identifier { get; private set; }

    public AdditionalAttribute[] AdditionalAttributes { get; private set; }

    public UpdateRequest(string identifier, AdditionalAttribute[] additionalAttributes)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);
        ArgumentNullException.ThrowIfNull(additionalAttributes);

        if (additionalAttributes.Length > 10)
        {
            throw new ArgumentException("Too many additional attributes provided. The maximum allowed is 10.", nameof(additionalAttributes));
        }

        Identifier = identifier;
        AdditionalAttributes = additionalAttributes;
    }
}