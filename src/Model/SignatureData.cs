namespace FrejaOrgId.Model;

public class SignatureData
{
    public string UserSignature { get; private set; }

    public string CertificateStatus { get; private set; }

    public SignatureData(string userSignature, string certificateStatus)
    {
        ArgumentException.ThrowIfNullOrEmpty(userSignature);
        ArgumentException.ThrowIfNullOrEmpty(certificateStatus);

        UserSignature = userSignature;
        CertificateStatus = certificateStatus;
    }
}
