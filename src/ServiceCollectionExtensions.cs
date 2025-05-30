﻿using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;

namespace FrejaOrgId;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrejaOrgIdClient(this IServiceCollection services, Action<FrejaOrgIdClientConfiguration> configure)
    {
        var configuration = new FrejaOrgIdClientConfiguration();
        configure(configuration);
        services.AddSingleton(configuration);

        services.AddScoped<IFrejaOrgIdClient, FrejaOrgIdClient>();

        return services;
    }

    public static IServiceCollection AddFrejaOrgIdHttpClient(this IServiceCollection services, X509Certificate2 certificate)
    {
        ArgumentNullException.ThrowIfNull(certificate);

        services.AddHttpClient(FrejaOrgIdClientConfiguration.DefaultHttpClientName)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);
                return handler;
            });

        return services;
    }
}
