using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Update;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace FrejaOrgId
{
    public enum FrejaEnvironment
    {
        Test,
        Production
    }

    public sealed class FrejaOrgIdApi : IDisposable, IFrejaOrgIdApi
    {
        private static readonly Dictionary<FrejaEnvironment, Uri> Endpoints = new()
        {
            { FrejaEnvironment.Test, new Uri("https://services.test.frejaeid.com/organisation/management/orgId/1.0/") },
            {
                FrejaEnvironment.Production,
                new Uri("https://services.prod.frejaeid.com/organisation/management/orgId/1.0/")
            }
        };

        private readonly ServiceProvider _serviceProvider;
        private readonly RsaSecurityKey _jwtSigningKey;
        private bool _disposedValue;

        internal FrejaOrgIdApi(FrejaEnvironment environment, X509Certificate2? apiCertificate,
            X509Certificate2 jwtSigningCertificate, bool skipCertificateCheck)
        {
            ArgumentNullException.ThrowIfNull(apiCertificate, nameof(apiCertificate));
            ArgumentNullException.ThrowIfNull(jwtSigningCertificate, nameof(jwtSigningCertificate));

            ServiceCollection services = new();

            SocketsHttpHandler handler = new()
            {
                SslOptions =
                {
                    ClientCertificates = [apiCertificate],
                }
            };

            if (skipCertificateCheck)
            {
                handler.SslOptions.RemoteCertificateValidationCallback = (message, cert, chain, errors) => true;
            }

            services.AddHttpClient<IApiService>(ApiService.HttpClientName,
                    client => { client.BaseAddress = Endpoints[environment]; })
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

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _serviceProvider?.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}