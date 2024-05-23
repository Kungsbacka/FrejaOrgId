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

        public OrgIdApi(Environment environment, X509Certificate2 apiCertificate, X509Certificate2 jwtSigningCertificate)
        {
            ServiceCollection services = new();
            services.AddHttpClient<IApiService>(client =>
            {
                client.BaseAddress = _endpoints[environment];
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ClientCertificates = { apiCertificate },
            });
            services.AddSingleton<IApiService, ApiService>();
            _serviceProvider = services.BuildServiceProvider();
            _jwtSigningKey = new RsaSecurityKey(jwtSigningCertificate.GetRSAPublicKey());
        }

        public async Task<InitAddResponse> InitAdd(InitAddRequest request) => 
            await StandardRequest<InitAddResponse, InitAddRequest>(request);

        public async Task<GetAllResponse> GetAll(GetAllRequest request) =>
            await StandardRequest<GetAllResponse, GetAllRequest>(request);

        public async Task<CancelAddResponse> CancelAdd(CancelAddRequest request) =>
            await StandardRequest<CancelAddResponse, CancelAddRequest>(request);

        public async Task<UpdateResponse> Update(UpdateRequest request) =>
            await StandardRequest<UpdateResponse, UpdateRequest>(request);

        public async Task<DeleteResponse> Delete(DeleteRequest request) =>
            await StandardRequest<DeleteResponse, DeleteRequest>(request);

        public async Task<GetOneResponse> GetOne(GetOneRequest request)
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
                throw new Exception("JWS signature is invalid");
            }
        }

        private async Task<TResponse> StandardRequest<TResponse, TRequest>(TRequest request)
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