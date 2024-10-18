using System.Security.Cryptography.X509Certificates;

namespace FrejaOrgId
{
    public class FrejaOrgIdApiBuilder
    {
        private FrejaEnvironment _environment;
        private X509Certificate2? _apiCertificate;
        private X509Certificate2? _jwtSigningCertificate;
        private bool _skipCertificateCheck = false;

        public FrejaOrgIdApiBuilder(FrejaEnvironment environment)
        {
            _environment = environment;
        }

        public FrejaOrgIdApiBuilder WithApiCertificate(X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificate, nameof(certificate));
            _apiCertificate = certificate;
            return this;
        }

        public FrejaOrgIdApiBuilder WithApiCertificate(StoreLocation storeLocation, string thumbprint)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(thumbprint, nameof(thumbprint));
            return WithApiCertificate(GetCertificateFromStore(storeLocation, thumbprint));
        }

        public FrejaOrgIdApiBuilder WithJwtSigningCertificate(X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificate, nameof(certificate));
            _jwtSigningCertificate = certificate;
            return this;
        }

        public FrejaOrgIdApiBuilder WithJwtSigningCertificate(StoreLocation storeLocation, string thumbprint)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(thumbprint, nameof(thumbprint));
            return WithJwtSigningCertificate(GetCertificateFromStore(storeLocation, thumbprint));
        }

        public FrejaOrgIdApiBuilder SkipCertificateCheck(bool skipCertificateCheck)
        {
            _skipCertificateCheck = skipCertificateCheck;
            return this;
        }

        public FrejaOrgIdApi Build()
        {
            if (_apiCertificate == null)
            {
                throw new InvalidOperationException("API certificate must be configured. Use WithApiCertificate().");
            }

            if (_jwtSigningCertificate == null)
            {
                throw new InvalidOperationException("JWT signing certificate must be configured. Use WithJwtSigningCertificate().");
            }

            return new FrejaOrgIdApi(_environment, _apiCertificate, _jwtSigningCertificate, _skipCertificateCheck);
        }
        private static X509Certificate2 GetCertificateFromStore(StoreLocation storeLocation, string thumbprint)
        {
            X509Store store = new(storeLocation);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certs.Count == 0)
            {
                throw new InvalidOperationException($"Cannot find a certificate '{thumbprint}' in '{storeLocation}'.");
            }
            return certs[0];
        }

    }
}
