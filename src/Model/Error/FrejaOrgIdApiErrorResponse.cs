using System.Text;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.Error
{

    public class FrejaOrgIdApiErrorResponse : IFrejaOrgIdApiErrorResponse
    {
        public int Code { get; private set; }
        public string Message { get; private set; }

        [JsonConstructor]
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