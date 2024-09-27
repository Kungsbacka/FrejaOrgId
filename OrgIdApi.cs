using FrejaOrgId.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace FrejaOrgId
{
    public class OrgIdApi : IDisposable
    {
        public enum Environment { Test, Production }

        private static readonly Dictionary<Environment, Uri> _endpoints = new()
        {
            { Environment.Test, new Uri("https://services.test.frejaeid.com/organisation/management/orgId/1.0/") },
            { Environment.Production, new Uri("https://services.prod.frejaeid.com/organisation/management/orgId/1.0/") }
        };

        private readonly ServiceProvider _serviceProvider;
        private readonly RsaSecurityKey _jwtSigningKey;
        private bool _disposedValue;

        public OrgIdApi(Environment environment, X509Certificate2 apiCertificate, X509Certificate2 jwtSigningCertificate, bool skipCertificateCheck = false) :
            this(environment, apiCertificate, apiCertificateStore: null, apiCertificateThumbprint: null, jwtSigningCertificate, skipCertificateCheck)
        { }

        public OrgIdApi(Environment environment, StoreLocation apiCertificateStore, string apiCertificateThumbprint, X509Certificate2 jwtSigningCertificate, bool skipCertificateCheck = false) :
                        this(environment, apiCertificate: null, apiCertificateStore, apiCertificateThumbprint, jwtSigningCertificate, skipCertificateCheck)

        { }

        private static X509Certificate2 GetCertificateFromStore(StoreLocation storeLocation, string thumbprint)
        {
            X509Store store = new(storeLocation);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certs.Count == 0)
            {
                throw new InvalidOperationException($"Cannot find a certificate with thumbprint {thumbprint}.");
            }
            return certs[0];
        }

        private OrgIdApi(Environment environment, X509Certificate2? apiCertificate, StoreLocation? apiCertificateStore, string? apiCertificateThumbprint, X509Certificate2 jwtSigningCertificate, bool skipCertificateCheck)
        {
            if (apiCertificate is null)
            {
                ArgumentNullException.ThrowIfNull(apiCertificateStore, nameof(apiCertificateStore));
                ArgumentNullException.ThrowIfNullOrEmpty(apiCertificateThumbprint, nameof(apiCertificateThumbprint));
                apiCertificate = GetCertificateFromStore(apiCertificateStore.Value, apiCertificateThumbprint);
            }
            
            ServiceCollection services = new();

            SocketsHttpHandler handler = new()
            {
                SslOptions =
                {
                    ClientCertificates = [ apiCertificate ],  
                }
            };

            if (skipCertificateCheck)
            {
                handler.SslOptions.RemoteCertificateValidationCallback = (message, cert, chain, errors) => true;
            }

            services.AddHttpClient<IApiService>(ApiService.HttpClientName, client =>
            {
                client.BaseAddress = _endpoints[environment];
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
            services.AddSingleton<IApiService, ApiService>();
            _serviceProvider = services.BuildServiceProvider();
            _jwtSigningKey = new RsaSecurityKey(jwtSigningCertificate.GetRSAPublicKey());
        }

        public async Task<InitAddResponse> InitAddAsync(InitAddRequest request) =>
            await SendRequestAsync<InitAddResponse, InitAddRequest>(request);

        public async Task<GetAllResponse> GetAllAsync(GetAllRequest request) =>
            await SendRequestAsync<GetAllResponse, GetAllRequest>(request);

        public async Task<CancelAddResponse> CancelAddAsync(CancelAddRequest request) =>
            await SendRequestAsync<CancelAddResponse, CancelAddRequest>(request);

        public async Task<UpdateResponse> UpdateAsync(UpdateRequest request) =>
            await SendRequestAsync<UpdateResponse, UpdateRequest>(request);

        public async Task<DeleteResponse> DeleteAsync(DeleteRequest request) =>
            await SendRequestAsync<DeleteResponse, DeleteRequest>(request);

        public async Task<GetOneResponse> GetOneAsync(GetOneRequest request)
        {
            IApiService apiService = _serviceProvider.GetRequiredService<IApiService>();
            GetOneResponse response = await apiService.SendRequestAsync<GetOneResponse, GetOneRequest>(request);
            if (response.Status == TransactionStatus.Approved)
            {
                ApprovedGetOneDetails? details = response.Details as ApprovedGetOneDetails;
                if (details?.OriginalJws == null)
                {
                    throw new Exception("OriginalJws should not be null.");
                }
                await ThrowIfInvalidJwsSignatureAsync(details.OriginalJws);
            }
            return response;
        }

        private async Task ThrowIfInvalidJwsSignatureAsync(string jwt)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = _jwtSigningKey,
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
            TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(jwt, validationParameters);
            if (!validationResult.IsValid)
            {
                throw new Exception("JWS signature is invalid")
                {
                    Data = { ["TokenValidationResult"] = validationResult }
                };
            }
        }

        private async Task<TResponse> SendRequestAsync<TResponse, TRequest>(TRequest request)
        {
            IApiService apiService = _serviceProvider.GetRequiredService<IApiService>();
            return await apiService.SendRequestAsync<TResponse, TRequest>(request);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _serviceProvider?.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}