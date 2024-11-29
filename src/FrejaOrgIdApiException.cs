using FrejaOrgId.Model.Error;

namespace FrejaOrgId
{
    public class FrejaOrgIdApiException : Exception
    {
        public int Code { get; } = 9999;

        public FrejaOrgIdApiException()
        {
        }

        public FrejaOrgIdApiException(string? message) : base(message)
        {
        }

        public FrejaOrgIdApiException(FrejaOrgIdApiErrorResponse errorResponse) : base(errorResponse.Message)
        {
            Code = errorResponse.Code;
        }
    }
}