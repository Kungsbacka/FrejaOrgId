using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetAll;

public class UserOrganisationId
{
    public string Title { get; private set; }

    public string IdentifierName { get; private set; }

    public string Identifier { get; private set; }

    [JsonConstructor]
    internal UserOrganisationId(string title, string identifierName, string identifier)
    {
        Title = title.ThrowIfNullOrEmpty(nameof(title));
        IdentifierName = identifierName;
        Identifier = identifier.ThrowIfNullOrEmpty(nameof(identifier));
    }
}
