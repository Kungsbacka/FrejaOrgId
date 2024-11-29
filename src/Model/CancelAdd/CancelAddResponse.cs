using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.CancelAdd;

public class CancelAddResponse : ICancelAddResponse
{
    [JsonConstructor]
    internal CancelAddResponse()
    {

    }
}
