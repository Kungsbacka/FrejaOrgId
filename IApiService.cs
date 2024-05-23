namespace FrejaOrgId
{
    internal interface IApiService
    {
        Task<TResponse> SendRequestAsync<TResponse, TRequest>(TRequest request);
    }
}
