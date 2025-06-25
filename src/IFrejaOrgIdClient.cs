using FrejaOrgId.Model.Request;
using FrejaOrgId.Model.Response;

namespace FrejaOrgId;

public interface IFrejaOrgIdClient
{
    Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken ct = default)
        where TRequest : FrejaApiRequest<TResponse>
        where TResponse : FrejaApiResponse;

    Task<TResponse> SendRequestAsync<TResponse>(FrejaApiRequest<TResponse> request, CancellationToken ct = default)
        where TResponse : FrejaApiResponse;
}