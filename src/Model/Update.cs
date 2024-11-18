namespace FrejaOrgId.Model
{
    public record UpdateRequest(string Identifier, AdditionalAttribute[] AdditionalAttributes);

    public record UpdateResponse(UpdateStatus UpdateStatus);

    public record UpdateStatus(int Added, int Updated, int Deleted);
}