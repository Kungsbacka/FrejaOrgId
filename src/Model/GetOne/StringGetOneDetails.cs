using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetOne;

public class StringGetOneDetails : GetOneDetailsBase
{
    public string Value { get; private set; }

    [JsonConstructor]
    internal StringGetOneDetails(string value)
    {
        Value = value.ThrowIfNullOrEmpty(nameof(value));
    }
}
