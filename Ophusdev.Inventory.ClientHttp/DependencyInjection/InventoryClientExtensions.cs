using Inventory.ClientHttp;
using Inventory.ClientHttp.Abstraction;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class InventoryClientExtensions {

    public static IServiceCollection AddOphusdevInventoryClient(this IServiceCollection services, IConfiguration configuration) {

        IConfigurationSection confSection = configuration.GetSection(InventoryClientOptions.SectionName);
        InventoryClientOptions options = confSection.Get<InventoryClientOptions>() ?? new();

        services.AddHttpClient<IClientHttp, ClientHttp>(o => {          
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

        return services;
    }
}
