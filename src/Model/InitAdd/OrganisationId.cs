using FrejaOrgId.Converters;
using FrejaOrgId.Model.Shared;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.InitAdd;

public class OrganisationId
{
    public string Title { get; private set; }

    public string IdentifierName { get; private set; }

    public string Identifier { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<IdentifierDisplayType[]>))]
    public IdentifierDisplayType[]? IdentifierDisplayTypes { get; private set; }

    public AdditionalAttribute[]? AdditionalAttributes { get; private set; }

    [JsonConstructor]
    public OrganisationId(string title, string identifierName, string identifier,
        IdentifierDisplayType[]? identifierDisplayTypes, AdditionalAttribute[]? additionalAttributes)
    {
        Title = title.ThrowIfNullOrEmpty(nameof(title));
        IdentifierName = identifierName.ThrowIfNullOrEmpty(nameof(identifierName));
        Identifier = identifier.ThrowIfNullOrEmpty(nameof(identifier));
        IdentifierDisplayTypes = identifierDisplayTypes;
        AdditionalAttributes = additionalAttributes.ThrowIfTooMany(maxCount: 10, nameof(additionalAttributes));
    }
}
