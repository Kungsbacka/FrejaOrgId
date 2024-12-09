using System.Text.Json.Serialization;

namespace FrejaOrgId.Model;

public class SsnUserInfo : UserInfoBase
{
    public string Country { get; private set; }
    public string Ssn { get; private set; }

    public SsnUserInfo(string country, string ssn)
    {
        ArgumentException.ThrowIfNullOrEmpty(country);
        ArgumentException.ThrowIfNullOrEmpty(ssn);

        Country = country;
        Ssn = ssn;
    }
}
