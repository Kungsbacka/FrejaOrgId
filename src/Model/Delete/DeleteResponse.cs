using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Delete;

public class DeleteResponse : IDeleteResponse
{
    [JsonConstructor]
    internal DeleteResponse()
    {
    
    }
}
