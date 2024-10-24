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

        public FrejaOrgIdApiException(FrejaOrgIdApiError error) : base(error.Message)
        {
            Code = error.Code;
        }
    }
}
