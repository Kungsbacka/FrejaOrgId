using System.Text;

namespace FrejaOrgId
{
    public class FrejaOrgIdApiErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        internal FrejaOrgIdApiErrorResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string? ToString()
        {
            StringBuilder sb = new();
            sb.Append(Code);
            sb.Append(": ");
            sb.Append(Message);
            return sb.ToString();
        }
    }
}