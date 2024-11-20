using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Shared;

public class SsnUserInfo : UserInfoBase
{
    public string Country { get; private set; }
    public string Ssn { get; private set; }

    [JsonConstructor]
    public SsnUserInfo(string country, string ssn)
    {
        Country = country.ThrowIfNullOrEmpty(nameof(country));
        Ssn = ssn.ThrowIfNullOrEmpty(nameof(ssn));
    }
}
