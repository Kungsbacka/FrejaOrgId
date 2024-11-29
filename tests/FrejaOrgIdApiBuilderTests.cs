using System.Security.Cryptography.X509Certificates;

namespace FrejaOrgId.Tests;

public class FrejaOrgIdApiBuilderTests
{
    [Fact]
    public void Build_ThrowsException_WhenApiCertificateNotConfigured()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test)
            .WithJwtSigningCertificate(CertificateHelper.GetTestCertificate());

        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("API certificate must be configured. Use WithApiCertificate().", exception.Message);
    }

    [Fact]
    public void Build_ThrowsException_WhenJwtSigningCertificateNotConfigured()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test)
            .WithApiCertificate(CertificateHelper.GetTestCertificate());

        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("JWT signing certificate must be configured. Use WithJwtSigningCertificate().", exception.Message);
    }

    [Fact]
    public void Build_ReturnsFrejaOrgIdApi_WhenAllCertificatesConfigured()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test)
            .WithApiCertificate(CertificateHelper.GetTestCertificate())
            .WithJwtSigningCertificate(CertificateHelper.GetTestCertificate());

        var api = builder.Build();

        Assert.NotNull(api);
        Assert.IsType<FrejaOrgIdApi>(api);
    }

    [Fact]
    public void WithApiCertificate_ThrowsException_WhenCertificateIsNull()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test);

        Assert.Throws<ArgumentNullException>(() => builder.WithApiCertificate(null!));
    }

    [Fact]
    public void WithJwtSigningCertificate_ThrowsException_WhenCertificateIsNull()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test);

        Assert.Throws<ArgumentNullException>(() => builder.WithJwtSigningCertificate(null!));
    }

    [Fact]
    public void WithApiCertificate_ThrowsException_WhenThumbprintIsNullOrEmpty()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test);

        Assert.Throws<ArgumentException>(() => builder.WithApiCertificate(StoreLocation.CurrentUser, ""));
        Assert.Throws<ArgumentNullException>(() => builder.WithApiCertificate(StoreLocation.CurrentUser, null!));
    }

    [Fact]
    public void WithJwtSigningCertificate_ThrowsException_WhenThumbprintIsNullOrEmpty()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test);

        Assert.Throws<ArgumentException>(() => builder.WithJwtSigningCertificate(StoreLocation.CurrentUser, ""));
        Assert.Throws<ArgumentNullException>(() => builder.WithJwtSigningCertificate(StoreLocation.CurrentUser, null!));
    }

    [Fact]
    public void SkipCertificateCheck_SetsSkipCertificateCheckFlag()
    {
        var builder = new FrejaOrgIdApiBuilder(FrejaEnvironment.Test);

        builder.SkipCertificateCheck(true);
        var api = builder.WithApiCertificate(CertificateHelper.GetTestCertificate())
            .WithJwtSigningCertificate(CertificateHelper.GetTestCertificate())
            .Build();

        Assert.NotNull(api);
    }
}
