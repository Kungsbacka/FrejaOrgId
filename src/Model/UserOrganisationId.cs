namespace FrejaOrgId.Model;

public class UserOrganisationId
{
    public string Title { get; private set; }

    public string IdentifierName { get; private set; }

    public string Identifier { get; private set; }

    public UserOrganisationId(string title, string identifierName, string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        ArgumentException.ThrowIfNullOrEmpty(identifierName);
        ArgumentException.ThrowIfNullOrEmpty(identifier);

        Title = title;
        IdentifierName = identifierName;
        Identifier = identifier;
    }
}
