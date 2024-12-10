# Freja Organisation eID library

C# library for Freja Organisation eID API. Can be used to send requests to the API and handle request/response serialization/deserialization.

## Example usage

### With a HttpClient created by the library

AuthCertificate is the certificate used to authenticate to The Freja Organisation eID API. jwtSigningCertificate is the certificate Freja uses to sign the JWT to create a JWS. When a InitAddReq is fetched from the API, the library will use this certificate to check the JWS signature if you not explicitly disable JWS signature check.

```C#
# Fetch certificates from secure storage. 
builder.Services.AddFrejaOrgIdHttpClient(authCertificate);
builder.Services.AddFrejaOrgIdClient(config =>
{
    config.Environment = FrejaEnvironment.Production;
    config.CopyJwtSigningKey(jwtSigningCertificate);
});
```

### With JWS signature validation disabled

```C#
builder.Services.AddFrejaOrgIdHttpClient(authCertificate);
builder.Services.AddFrejaOrgIdClient(config =>
{
    config.Environment = FrejaEnvironment.Test;
    config.DisableJwtSignatureValidation = true;
});
```

### With your own HttpClient

```C#
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
