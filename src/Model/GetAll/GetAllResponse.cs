using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetAll;

public class GetAllResponse
{
    public UserInfo[] UserInfos { get; private set; }

    [JsonConstructor]
    internal GetAllResponse(UserInfo[] userInfos)
    {
        UserInfos = userInfos.ThrowIfNull(nameof(userInfos));
    }
}
