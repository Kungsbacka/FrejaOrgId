namespace FrejaOrgId.Tests;

using FrejaOrgId.Model.CancelAdd;
using FrejaOrgId.Model.Delete;
using FrejaOrgId.Model.GetAll;
using FrejaOrgId.Model.GetOne;
using FrejaOrgId.Model.InitAdd;
using FrejaOrgId.Model.Shared;
using FrejaOrgId.Model.Update;
using System;
using Xunit;

public class ModelTests
{
    [Fact]
    public void CancelAddRequest_ThrowsException_WhenOrgIdRefIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CancelAddRequest(null!));
        Assert.Throws<ArgumentException>(() => new CancelAddRequest(string.Empty));
    }

    [Fact]
    public void DeleteRequest_ThrowsException_WhenIdentifierIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new DeleteRequest(null!));
        Assert.Throws<ArgumentException>(() => new DeleteRequest(string.Empty));
    }

    [Fact]
    public void InitAddRequest_SetsExpiryToZero_WhenExpirationIsLessThanOrEqualToZero()
    {
        var userInfo = new StringUserInfo("test");
        var organisationId = new OrganisationId("Org", "IdName", "Id", null, null);

        var request = new InitAddRequest(UserInfoType.Email, userInfo, organisationId, MinRegistrationLevel.Extended, -1);
        Assert.Equal(0, request.Expiry);

        request = new InitAddRequest(UserInfoType.Email, userInfo, organisationId, MinRegistrationLevel.Extended, 0);
        Assert.Equal(0, request.Expiry);
    }

    [Fact]
    public void InitAddRequest_ThrowsArgumentException_WhenExpirationIsOutOfRange()
    {
        var userInfo = new StringUserInfo("test");
        var organisationId = new OrganisationId("Org", "IdName", "Id", null, null);

        Assert.Throws<ArgumentException>(() =>
            new InitAddRequest(UserInfoType.Email, userInfo, organisationId, MinRegistrationLevel.Extended, 43201));
    }

    [Fact]
    public void InitAddRequest_ThrowsInvalidOperationException_WhenUserInfoTypeMismatch()
    {
        var stringUserInfo = new StringUserInfo("test");
        var ssnUserInfo = new SsnUserInfo("SE", "198001010001");
        var organisationId = new OrganisationId("Org", "IdName", "Id", null, null);

        Assert.Throws<InvalidOperationException>(() =>
            new InitAddRequest(UserInfoType.Ssn, stringUserInfo, organisationId, MinRegistrationLevel.Extended, 10));

        Assert.Throws<InvalidOperationException>(() =>
            new InitAddRequest(UserInfoType.Email, ssnUserInfo, organisationId, MinRegistrationLevel.Extended, 10));
    }

    [Fact]
    public void InitAddRequest_SetsExpiryCorrectly_WhenExpirationValid()
    {
        var userInfo = new StringUserInfo("test");
        var organisationId = new OrganisationId("Org", "IdName", "Id", null, null);
        var expirationInMinutes = 10;
        var request = new InitAddRequest(UserInfoType.Email, userInfo, organisationId, MinRegistrationLevel.Extended, expirationInMinutes);

        var expectedExpiryLowerBound = DateTimeOffset.Now.AddMinutes(expirationInMinutes).ToUnixTimeMilliseconds() - 1000;
        var expectedExpiryUpperBound = DateTimeOffset.Now.AddMinutes(expirationInMinutes).ToUnixTimeMilliseconds() + 1000;

        Assert.InRange(request.Expiry, expectedExpiryLowerBound, expectedExpiryUpperBound);
    }

    [Fact]
    public void ApprovedGetOneDetails_SetOriginalJws_ThrowsInvalidOperationException_WhenAlreadySet()
    {
        var details = new ApprovedGetOneDetails(
            "OrgRef",
            TransactionStatus.Approved,
            UserInfoType.Email,
            new StringUserInfo("test"),
            MinRegistrationLevel.Extended,
            DateTime.UtcNow,
            SignatureType.Simple,
            new SignatureData("Signature", "Valid"));

        details.SetOriginalJws("test-jws");

        Assert.Throws<InvalidOperationException>(() => details.SetOriginalJws("another-jws"));
    }

    [Fact]
    public void UserOrganisationId_ThrowsException_WhenRequiredFieldsAreNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new UserOrganisationId(null!, "Name", "Id"));
        Assert.Throws<ArgumentNullException>(() => new UserOrganisationId("Title", "Name", null!));
        Assert.Throws<ArgumentException>(() => new UserOrganisationId("", "Name", "Id"));
        Assert.Throws<ArgumentException>(() => new UserOrganisationId("Title", "Name", ""));
    }

    [Fact]
    public void AdditionalAttribute_ThrowsException_WhenRequiredFieldsAreNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new AdditionalAttribute(null!, "DisplayText", "Value"));
        Assert.Throws<ArgumentNullException>(() => new AdditionalAttribute("Key", null!, "Value"));
        Assert.Throws<ArgumentNullException>(() => new AdditionalAttribute("Key", "DisplayText", null!));
        Assert.Throws<ArgumentException>(() => new AdditionalAttribute("", "DisplayText", "Value"));
        Assert.Throws<ArgumentException>(() => new AdditionalAttribute("Key", "", "Value"));
        Assert.Throws<ArgumentException>(() => new AdditionalAttribute("Key", "DisplayText", ""));
    }

    [Fact]
    public void UpdateRequest_ThrowsException_WhenIdentifierIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new UpdateRequest(null!, Array.Empty<AdditionalAttribute>()));
        Assert.Throws<ArgumentException>(() => new UpdateRequest(string.Empty, Array.Empty<AdditionalAttribute>()));
    }

    [Fact]
    public void UpdateRequest_ThrowsArgumentException_WhenAdditionalAttributesExceedLimit()
    {
        var attributes = new AdditionalAttribute[11];
        for (int i = 0; i < 11; i++)
        {
            attributes[i] = new AdditionalAttribute($"Key{i}", $"DisplayText{i}", $"Value{i}");
        }

        Assert.Throws<ArgumentException>(() => new UpdateRequest("Identifier", attributes));
    }
}
