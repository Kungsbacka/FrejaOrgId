namespace FrejaOrgId.Model;

public class UserSsn
{
    public string Ssn { get; private set; }

    public string Country { get; private set; }

    public UserSsn(string ssn, string country)
    {
        ArgumentException.ThrowIfNullOrEmpty(ssn);
        ArgumentException.ThrowIfNullOrEmpty(country);

        Ssn = ssn;
        Country = country;
    }
}
