namespace FrejaOrgId.Model.Error
{
    public interface IFrejaOrgIdApiErrorResponse
    {
        int Code { get; }
        string Message { get; }
    }
}