using System.Text;

namespace FrejaOrgId.Model.Response;

public class ErrorResponse : FrejaApiResponse
{
    public int Code { get; private set; }
    public string Message { get; private set; }

    public ErrorResponse(int code, string message)
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
