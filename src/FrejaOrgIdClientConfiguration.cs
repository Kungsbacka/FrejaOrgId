using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace FrejaOrgId;

public sealed class FrejaOrgIdClientConfiguration
{

    internal static readonly Uri TestEnvironmentBaseAddress = new("https://services.test.frejaeid.com/organisation/management/orgId/1.0/");
    internal static readonly Uri ProductionEnvironmentBaseAddress = new("https://services.prod.frejaeid.com/organisation/management/orgId/1.0/");

    public const string DefaultHttpClientName = "FrejaOrgIdHttpClient";

    public FrejaEnvironment Environment { get; set; } = FrejaEnvironment.Test;
    public RsaSecurityKey? JwtSigningKey { get; set; }
    public bool DisableJwtSignatureValidation { get; set; } = false;
    public string HttpClientName { get; set; } = DefaultHttpClientName;

    public void CopyJwtSigningKey(X509Certificate2 certificate)
    {
        JwtSigningKey = new RsaSecurityKey(certificate.GetRSAPublicKey());
    }
}
