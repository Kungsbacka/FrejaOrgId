using FrejaOrgId.Model;
using FrejaOrgId.Model.Request;
using FrejaOrgId.Model.Response;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using System.Buffers.Text;

namespace FrejaOrgId;

public sealed class FrejaOrgIdClient : IFrejaOrgIdClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly FrejaOrgIdClientConfiguration _configuration;

    public FrejaOrgIdClient(IHttpClientFactory httpClientFactory, FrejaOrgIdClientConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;

        _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : FrejaApiRequest<TResponse>
        where TResponse : FrejaApiResponse
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        using var content = BuildBase64EncodedStringContent<TRequest, TResponse>(request);
        using var responseMessage = await GetHttpClient().PostAsync(request.Endpoint, content);
        await using var contentStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, _serializerOptions)
                ?? throw new JsonException("Failed to deserialize response");

            if (response is GetOneResponse getOneResponse)
            {
                await ValidateGetOneResponseAsync(getOneResponse);
            }
            return response;
        }

        var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>()
            ?? throw new FrejaOrgIdApiException();
        throw new FrejaOrgIdApiException(error);
    }

    public async Task<TResponse> SendRequestAsync<TResponse>(FrejaApiRequest<TResponse> request)
    where TResponse : FrejaApiResponse
    {
        return await SendRequestAsync<FrejaApiRequest<TResponse>, TResponse>(request);
    }

    private async Task ValidateGetOneResponseAsync(GetOneResponse response)
    {
        if (response.Status == TransactionStatus.Approved)
        {
            ApprovedGetOneDetails? details = response.Details as ApprovedGetOneDetails;
            if (details?.OriginalJws == null)
            {
                throw new Exception("OriginalJws should not be null.");
            }
            if (!_configuration.DisableJwtSignatureValidation && _configuration.JwtSigningKey != null)
            {
                await ThrowIfInvalidJwsSignatureAsync(details.OriginalJws);
            }
        }
    }

    private HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient(_configuration.HttpClientName);
        httpClient.BaseAddress = _configuration.Environment == FrejaEnvironment.Test
            ? FrejaOrgIdClientConfiguration.TestEnvironmentBaseAddress
            : FrejaOrgIdClientConfiguration.ProductionEnvironmentBaseAddress;
        return httpClient;
    }

    private async Task ThrowIfInvalidJwsSignatureAsync(string jwt)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = _configuration.JwtSigningKey,
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

    private StringContent? BuildBase64EncodedStringContent<TRequest, TResponse>(TRequest request)
        where TRequest : FrejaApiRequest<TResponse>
        where TResponse : FrejaApiResponse
    {
        if (request.Action == null)
        {
            return null;
        }

        StringBuilder sb = new(request.Action);
        sb.Append('=');
        Span<byte> bytes = JsonSerializer.SerializeToUtf8Bytes(request, request.GetType(), _serializerOptions);
        sb.Append(Convert.ToBase64String(bytes));
        return new StringContent(sb.ToString());
    }
}