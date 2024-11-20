using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Update;

public class UpdateResponse
{
    public UpdateStatus UpdateStatus { get; private set; }

    [JsonConstructor]
    internal UpdateResponse(UpdateStatus updateStatus)
    {
        UpdateStatus = updateStatus;
    }
}