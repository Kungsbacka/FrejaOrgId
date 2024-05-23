using System.Text;
using System.Text.Json;

namespace FrejaOrgId
{
    internal class ApiService : IApiService
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> SendRequestAsync<TResponse, TRequest>(TRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ApiAction apiAction = ApiAction.Get<TRequest>();
            using StringContent? content = BuildBase64EncodedContent(request, apiAction.Name);
            using HttpResponseMessage? responseMessage = await _httpClientFactory.CreateClient().PostAsync(apiAction.Path, content);
            using Stream contentStream = await responseMessage.Content.ReadAsStreamAsync();
            if (responseMessage.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<TResponse>(contentStream, _serializerOptions)
                    ?? throw new JsonException("Failed to deserialize response");
            }
            var error = await responseMessage.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        
        private static StringContent? BuildBase64EncodedContent<T>(T request, string? actionName)
        {
            if (actionName == null)
            {
                return null;
            }
            StringBuilder sb = new(actionName);
            sb.Append('=');
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(request, _serializerOptions);
            sb.Append(Convert.ToBase64String(bytes));
            return new StringContent(sb.ToString());
        }
    }
}
