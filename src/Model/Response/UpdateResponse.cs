namespace FrejaOrgId.Model.Response;

public class UpdateResponse : FrejaApiResponse
{
    public UpdateStatus UpdateStatus { get; private set; }

    public UpdateResponse(UpdateStatus updateStatus)
    {
        UpdateStatus = updateStatus;
    }
}