namespace FrejaOrgId.Model;

public class StringGetOneDetails : GetOneDetailsBase
{
    public string Value { get; private set; }

    public StringGetOneDetails(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        
        Value = value;
    }
}
