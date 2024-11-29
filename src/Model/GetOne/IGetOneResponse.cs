namespace FrejaOrgId.Model.GetOne;

public interface IGetOneResponse
{
    GetOneDetailsBase Details { get; }
    string OrgIdRef { get; }
    TransactionStatus Status { get; }
}
