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
        Task<ICancelAddResponse> CancelAddAsync(ICancelAddRequest request);
        Task<IDeleteResponse> DeleteAsync(IDeleteRequest request);
        Task<IGetAllResponse> GetAllAsync(IGetAllRequest request);
        Task<IGetOneResponse> GetOneAsync(IGetOneRequest request);
        Task<IInitAddResponse> InitAddAsync(IInitAddRequest request);
        Task<IUpdateResponse> UpdateAsync(IUpdateRequest request);
    }
}