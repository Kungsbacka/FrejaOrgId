namespace FrejaOrgId.Model.Response;

public class GetAllResponse : FrejaApiResponse
{
    public UserInfo[] UserInfos { get; private set; }

    public GetAllResponse(UserInfo[] userInfos)
    {
        ArgumentNullException.ThrowIfNull(userInfos);

        UserInfos = userInfos;
    }
}
