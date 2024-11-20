using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetAll;

public class UserSsn
{
    public string Ssn { get; private set; }

    public string Country { get; private set; }

    [JsonConstructor]
    internal UserSsn(string ssn, string country)
    {
        Ssn = ssn.ThrowIfNullOrEmpty(nameof(ssn));
        Country = country.ThrowIfNullOrEmpty(nameof(country));
    }
}
