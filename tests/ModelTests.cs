using FrejaOrgId.Model.Request;
using FrejaOrgId.Model;
using System.Text.Json;

namespace FrejaOrgId.Tests;

public class ModelTests
{
    [Fact]
    public void CancelAddRequest_ShouldSetOrgIdRef()
    {
        string orgIdRef = "sample-org-id-ref";

        var request = new CancelAddRequest(orgIdRef);

        Assert.Equal(orgIdRef, request.OrgIdRef);
        Assert.Equal("cancelAdd", request.Endpoint);
        Assert.Equal("cancelAddOrganisationIdRequest", request.Action);
    }

    [Fact]
    public void CancelAddRequest_ShouldThrowExceptionIfOrgIdRefIsNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => new CancelAddRequest(string.Empty));
    }

    [Fact]
    public void DeleteRequest_Should_Set_Identifier()
    {
        string identifier = "sample-identifier";

        var request = new DeleteRequest(identifier);

        Assert.Equal(identifier, request.Identifier);
        Assert.Equal("delete", request.Endpoint);
        Assert.Equal("deleteOrganisationIdRequest", request.Action);
    }

    [Fact]
    public void DeleteRequest_ShouldThrowExceptionIfIdentifierIsNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => new DeleteRequest(string.Empty));
    }

    [Fact]
    public void GetAllRequest_Should_Set_Endpoint_And_Action()
    {
        var request = new GetAllRequest();

        Assert.Equal("users/getAll", request.Endpoint);
        Assert.Null(request.Action);
    }

    [Fact]
    public void GetOneRequest_Should_Set_OrgIdRef()
    {
        string orgIdRef = "sample-org-id-ref";

        var request = new GetOneRequest(orgIdRef);

        Assert.Equal(orgIdRef, request.OrgIdRef);
        Assert.Equal("getOneResult", request.Endpoint);
        Assert.Equal("getOneOrganisationIdResultRequest", request.Action);
    }

    [Fact]
    public void GetOneRequest_Should_Throw_Exception_If_OrgIdRef_Is_NullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => new GetOneRequest(string.Empty));
    }

    [Fact]
    public void InitAddRequest_Should_Set_Properties()
    {
        var userInfoType = UserInfoType.Phone;
        var userInfo = new StringUserInfo("+1234567890");
        var organisationId = new OrganisationId("Title", "IdentifierName", "Identifier", null, null);
        var minRegistrationLevel = MinRegistrationLevel.Extended;
        int expirationInMinutes = 30;

        var request = new InitAddRequest(userInfoType, userInfo, organisationId, minRegistrationLevel, expirationInMinutes);

        Assert.Equal(userInfoType, request.UserInfoType);
        Assert.Equal(userInfo, request.UserInfo);
        Assert.Equal(organisationId, request.OrganisationId);
        Assert.Equal(minRegistrationLevel, request.MinRegistrationLevel);
        Assert.True(request.Expiry > 0);
    }

    [Fact]
    public void InitAddRequest_Should_Throw_Exception_For_Invalid_Expiration()
    {
        var userInfoType = UserInfoType.Phone;
        var userInfo = new StringUserInfo("+1234567890");
        var organisationId = new OrganisationId("Title", "IdentifierName", "Identifier", null, null);

        Assert.Throws<ArgumentException>(() => new InitAddRequest(userInfoType, userInfo, organisationId, MinRegistrationLevel.Extended, 1));
    }

    [Fact]
    public void InitAddRequest_Should_Serialize_Correctly()
    {
        var userInfoType = UserInfoType.Ssn;
        var userInfo = new SsnUserInfo("SE", "198001010000");
        var organisationId = new OrganisationId("Title", "IdentifierName", "Identifier", null, null);
        var minRegistrationLevel = MinRegistrationLevel.Extended;
        int expirationInMinutes = 0;

        var request = new InitAddRequest(userInfoType, userInfo, organisationId, minRegistrationLevel, expirationInMinutes);

        var expectedJson = "{\"UserInfoType\":\"SSN\",\"UserInfo\":\"eyJDb3VudHJ5IjoiU0UiLCJTc24iOiIxOTgwMDEwMTAwMDAifQ==\",\"MinRegistrationLevel\":\"EXTENDED\",\"OrganisationId\":{\"Title\":\"Title\",\"IdentifierName\":\"IdentifierName\",\"Identifier\":\"Identifier\",\"IdentifierDisplayTypes\":null,\"AdditionalAttributes\":null}}";

        var actualjson = JsonSerializer.Serialize(request);

        Assert.Equal(expectedJson, actualjson);
    }

    [Fact]
    public void UpdateRequest_Should_Set_Properties()
    {
        var identifier = "sample-identifier";
        var additionalAttributes = new[]
        {
            new AdditionalAttribute("Key1", "DisplayText1", "Value1"),
            new AdditionalAttribute("Key2", "DisplayText2", "Value2")
        };

        var request = new UpdateRequest(identifier, additionalAttributes);

        Assert.Equal(identifier, request.Identifier);
        Assert.Equal(additionalAttributes, request.AdditionalAttributes);
        Assert.Equal("update", request.Endpoint);
        Assert.Equal("updateOrganisationIdRequest", request.Action);
    }

    [Fact]
    public void UpdateRequest_Should_Throw_Exception_If_Too_Many_Attributes()
    {
        var identifier = "sample-identifier";
        var tooManyAttributes = new AdditionalAttribute[11];
        for (int i = 0; i < 11; i++)
        {
            tooManyAttributes[i] = new AdditionalAttribute($"Key{i}", $"DisplayText{i}", $"Value{i}");
        }

        Assert.Throws<ArgumentException>(() => new UpdateRequest(identifier, tooManyAttributes));
    }
}
