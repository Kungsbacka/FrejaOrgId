using FrejaOrgId.Converters;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model;

public class OrganisationId
{
    public string Title { get; private set; }

    public string IdentifierName { get; private set; }

    public string Identifier { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<IdentifierDisplayType[]>))]
    public IdentifierDisplayType[]? IdentifierDisplayTypes { get; private set; }

    public AdditionalAttribute[]? AdditionalAttributes { get; private set; }

    public OrganisationId(string title, string identifierName, string identifier,
        IdentifierDisplayType[]? identifierDisplayTypes, AdditionalAttribute[]? additionalAttributes)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        ArgumentException.ThrowIfNullOrEmpty(identifierName);
        ArgumentException.ThrowIfNullOrEmpty(identifier);
        if (additionalAttributes != null && additionalAttributes.Length > 10)
        {
            throw new ArgumentException("Too many additional attributes provided. The maximum allowed is 10.", nameof(additionalAttributes));
        }

        Title = title;
        IdentifierName = identifierName;
        Identifier = identifier;
        IdentifierDisplayTypes = identifierDisplayTypes;
        AdditionalAttributes = additionalAttributes;
    }
}
