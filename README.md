# Freja Organisation eID library

C# library for the Freja eID [Organisation ID service][]. Can be used to send requests to the service and handle request/response serialization/deserialization.

[Organisation ID service]: https://frejaeid.atlassian.net/wiki/spaces/DOC/pages/2162756/Organisation+ID+Service

## Example usage

### With an HttpClient created by the library

AuthCertificate is the certificate used to authenticate to the Freja eID Organisation ID service. jwtSigningCertificate is the certificate the serivce uses to sign a JWS. When an Organisation ID Result is fetched from the service (with getOneResult), the library will use this certificate to validate the JWS signature from the result, if you do not explicitly disable the JWS signature validation.

```csharp
// Fetch authCertificate and jwtSigningCertificate, and then do
builder.Services.AddFrejaOrgIdHttpClient(authCertificate);
builder.Services.AddFrejaOrgIdClient(config =>
{
    config.Environment = FrejaEnvironment.Production;
    config.CopyJwtSigningKey(jwtSigningCertificate);
});
```

### With JWS signature validation disabled

```csharp
builder.Services.AddFrejaOrgIdHttpClient(authCertificate);
builder.Services.AddFrejaOrgIdClient(config =>
{
    config.Environment = FrejaEnvironment.Test;
    config.DisableJwtSignatureValidation = true;
});
```

### With your own HttpClient

```csharp
builder.Services.AddFrejaOrgIdClient(config =>
{
    config.Environment = FrejaEnvironment.Production;
    config.HttpClientName = "MyOwnHttpClient";
    config.CopyJwtSigningKey(jwtSigningCertificate);
});
```

## License

This project is licensed under the [MIT License][].

[MIT license]: https://github.com/Kungsbacka/FrejaOrgId/tree/master/LICENSE.txt

---

© Kungsbacka kommun. All rights reserved.
