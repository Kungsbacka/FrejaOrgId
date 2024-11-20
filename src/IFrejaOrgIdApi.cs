using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Update;

namespace FrejaOrgId
{
    public interface IFrejaOrgIdApi
    {
        Task<CancelAddResponse> CancelAddAsync(CancelAddRequest request);
        Task<DeleteResponse> DeleteAsync(DeleteRequest request);
        Task<GetAllResponse> GetAllAsync(GetAllRequest request);
        Task<GetOneResponse> GetOneAsync(GetOneRequest request);
        Task<InitAddResponse> InitAddAsync(InitAddRequest request);
        Task<UpdateResponse> UpdateAsync(UpdateRequest request);
    }
}