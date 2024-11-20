using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetOne;

public class SignatureData
{
    public string UserSignature { get; private set; }

    public string CertificateStatus { get; private set; }

    [JsonConstructor]
    internal SignatureData(string userSignature, string certificateStatus)
    {
        UserSignature = userSignature.ThrowIfNullOrEmpty(nameof(userSignature));
        CertificateStatus = certificateStatus.ThrowIfNullOrEmpty(nameof(certificateStatus));
    }
}
