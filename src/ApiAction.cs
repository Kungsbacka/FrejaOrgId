using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Update;
using System.Collections.Frozen;

namespace FrejaOrgId
{
    internal class ApiAction(string? name, string path, Type responseType)
    {
        public string? Name { get; } = name;
        public string Path { get; } = path;
        public Type ResponseType { get; } = responseType;

        private static readonly FrozenDictionary<Type, ApiAction> ApiActionDict = new Dictionary<Type, ApiAction>()
        {
            { typeof(IInitAddRequest), new ApiAction("initAddOrganisationIdRequest", "initAdd", typeof(InitAddResponse)) },
            { typeof(IGetOneRequest), new ApiAction("getOneOrganisationIdResultRequest", "getOneResult", typeof(GetOneResponse)) },
            { typeof(ICancelAddRequest), new ApiAction("cancelAddOrganisationIdRequest", "cancelAdd", typeof(CancelAddResponse)) },
            { typeof(IUpdateRequest), new ApiAction("updateOrganisationIdRequest", "update", typeof(UpdateResponse)) },
            { typeof(IDeleteRequest), new ApiAction("deleteOrganisationIdRequest", "delete", typeof(DeleteResponse)) },
            { typeof(IGetAllRequest), new ApiAction(null, "users/getAll", typeof(GetAllResponse)) },
        }.ToFrozenDictionary();

        public static ApiAction Get<T>()
        {
            if (ApiActionDict.TryGetValue(typeof(T), out ApiAction? action))
            {
                return action;
            }

            throw new ArgumentException("Unknown API action");
        }
    }
}