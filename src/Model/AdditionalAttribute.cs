using System.Text.Json.Serialization;

namespace FrejaOrgId.Model;

public class AdditionalAttribute
{
    public string Key { get; private set; }

    public string DisplayText { get; private set; }

    public string Value { get; private set; }

    [JsonConstructor]
    public AdditionalAttribute(string key, string displayText, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        ArgumentException.ThrowIfNullOrEmpty(displayText);
        ArgumentException.ThrowIfNullOrEmpty(value);

        Key = key;
        DisplayText = displayText;
        Value = value;
    }
}
