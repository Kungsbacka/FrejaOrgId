namespace FrejaOrgId.Model;

public class StringUserInfo : UserInfoBase
{
    public string Value { get; private set; }

    public StringUserInfo(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        Value = value;
    }
}
