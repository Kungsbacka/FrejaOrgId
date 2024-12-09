namespace FrejaOrgId.Model;

public class UpdateStatus
{
    public int Added { get; private set; }

    public int Updated { get; private set; }

    public int Deleted { get; private set; }

    public UpdateStatus(int added, int updated, int deleted)
    {
        Added = added;
        Updated = updated;
        Deleted = deleted;
    }
}