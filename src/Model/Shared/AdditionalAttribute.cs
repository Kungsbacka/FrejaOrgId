using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Shared;

public class AdditionalAttribute
{
    public string Key { get; private set; }

    public string DisplayText { get; private set; }

    public string Value { get; private set; }

    [JsonConstructor]
    public AdditionalAttribute(string key, string displayText, string value)
    {
        Key = key.ThrowIfNullOrEmpty(nameof(key));
        DisplayText = displayText.ThrowIfNullOrEmpty(nameof(displayText));
        Value = value.ThrowIfNullOrEmpty(nameof(value));
    }
}
