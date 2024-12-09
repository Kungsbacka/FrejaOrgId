using FrejaOrgId.Model.Response;

namespace FrejaOrgId
{
    public sealed class FrejaOrgIdApiException : Exception
    {
        public int Code { get; } = 9999;

        public FrejaOrgIdApiException()
        {
        }

        public FrejaOrgIdApiException(string? message) : base(message)
        {
        }

        public FrejaOrgIdApiException(ErrorResponse errorResponse) : base(errorResponse.Message)
        {
            Code = errorResponse.Code;
        }
    }
}