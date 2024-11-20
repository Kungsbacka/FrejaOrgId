using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Shared;

public class StringUserInfo : UserInfoBase
{
    public string Value { get; private set; }

    [JsonConstructor]
    public StringUserInfo(string value)
    {
        Value = value.ThrowIfNullOrEmpty(nameof(value));
    }
}
