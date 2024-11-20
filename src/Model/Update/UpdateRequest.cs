using FrejaOrgId.Model.Shared;

namespace FrejaOrgId.Model.Update;

public class UpdateRequest
{
    public string Identifier { get; private set; }

    public AdditionalAttribute[] AdditionalAttributes { get; private set; }

    public UpdateRequest(string identifier, AdditionalAttribute[] additionalAttributes)
    {
        Identifier = identifier.ThrowIfNullOrEmpty(nameof(identifier));
        AdditionalAttributes = additionalAttributes.ThrowIfNullOrTooMany(maxCount: 10, nameof(additionalAttributes));
    }
}