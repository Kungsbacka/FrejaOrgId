using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Update;

public class UpdateStatus
{
    public int Added { get; private set; }

    public int Updated { get; private set; }

    public int Deleted { get; private set; }

    [JsonConstructor]
    internal UpdateStatus(int added, int updated, int deleted)
    {
        Added = added;
        Updated = updated;
        Deleted = deleted;
    }
}