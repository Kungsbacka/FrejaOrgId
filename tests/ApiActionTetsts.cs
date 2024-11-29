using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Update;

namespace FrejaOrgId.Tests;

public class ApiActionTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        string? name = "testAction";
        string path = "test/path";
        Type responseType = typeof(string);

        // Act
        var apiAction = new ApiAction(name, path, responseType);

        // Assert
        Assert.Equal(name, apiAction.Name);
        Assert.Equal(path, apiAction.Path);
        Assert.Equal(responseType, apiAction.ResponseType);
    }

    [Fact]
    public void Get_ValidType_ReturnsCorrectApiAction()
    {
        // Act
        var result = ApiAction.Get<IInitAddRequest>();

        // Assert
        Assert.Equal("initAddOrganisationIdRequest", result.Name);
        Assert.Equal("initAdd", result.Path);
        Assert.Equal(typeof(InitAddResponse), result.ResponseType);
    }

    [Fact]
    public void Get_AnotherValidType_ReturnsCorrectApiAction()
    {
        // Act
        var result = ApiAction.Get<IGetOneRequest>();

        // Assert
        Assert.Equal("getOneOrganisationIdResultRequest", result.Name);
        Assert.Equal("getOneResult", result.Path);
        Assert.Equal(typeof(GetOneResponse), result.ResponseType);
    }

    [Fact]
    public void Get_CancelAddRequest_ReturnsCorrectApiAction()
    {
        // Act
        var result = ApiAction.Get<ICancelAddRequest>();

        // Assert
        Assert.Equal("cancelAddOrganisationIdRequest", result.Name);
        Assert.Equal("cancelAdd", result.Path);
        Assert.Equal(typeof(CancelAddResponse), result.ResponseType);
    }

    [Fact]
    public void Get_UpdateRequest_ReturnsCorrectApiAction()
    {
        // Act
        var result = ApiAction.Get<IUpdateRequest>();

        // Assert
        Assert.Equal("updateOrganisationIdRequest", result.Name);
        Assert.Equal("update", result.Path);
        Assert.Equal(typeof(UpdateResponse), result.ResponseType);
    }

    [Fact]
    public void Get_DeleteRequest_ReturnsCorrectApiAction()
    {
        // Act
        var result = ApiAction.Get<IDeleteRequest>();

        // Assert
        Assert.Equal("deleteOrganisationIdRequest", result.Name);
        Assert.Equal("delete", result.Path);
        Assert.Equal(typeof(DeleteResponse), result.ResponseType);
    }

    [Fact]
    public void Get_GetAllRequest_ReturnsCorrectApiActionWithNullName()
    {
        // Act
        var result = ApiAction.Get<IGetAllRequest>();

        // Assert
        Assert.Null(result.Name);
        Assert.Equal("users/getAll", result.Path);
        Assert.Equal(typeof(GetAllResponse), result.ResponseType);
    }

    [Fact]
    public void Get_InvalidType_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ApiAction.Get<IUnknownRequest>());
    }
}

internal interface IUnknownRequest { }
