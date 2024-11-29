namespace FrejaOrgId.Model.Delete;

public class DeleteRequest : IDeleteRequest
{
    public string Identifier { get; private set; }

    public DeleteRequest(string identifier)
    {
        Identifier = identifier.ThrowIfNullOrEmpty(nameof(identifier));
    }
}