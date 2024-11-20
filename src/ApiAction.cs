using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Update;
using System.Collections.Frozen;

namespace FrejaOrgId
{
    internal class ApiAction(string? name, string path)
    {
        public string? Name { get; } = name;
        public string Path { get; } = path;

        private static readonly FrozenDictionary<Type, ApiAction> ApiActionDict = new Dictionary<Type, ApiAction>()
        {
            { typeof(InitAddRequest), new ApiAction("initAddOrganisationIdRequest", "initAdd") },
            { typeof(GetOneRequest), new ApiAction("getOneOrganisationIdResultRequest", "getOneResult") },
            { typeof(CancelAddRequest), new ApiAction("cancelAddOrganisationIdRequest", "cancelAdd") },
            { typeof(UpdateRequest), new ApiAction("updateOrganisationIdRequest", "update") },
            { typeof(DeleteRequest), new ApiAction("deleteOrganisationIdRequest", "delete") },
            { typeof(GetAllRequest), new ApiAction(null, "users/getAll") },
        }.ToFrozenDictionary();

        public static ApiAction Get<T>()
        {
            if (ApiActionDict.TryGetValue(typeof(T), out ApiAction? action))
            {
                return action!;
            }

            throw new ArgumentException("Unknown API action");
        }
    }
}