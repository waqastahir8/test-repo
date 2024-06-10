using AmeriCorps.Users.Data.Deployment;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
    {
        builder
        .AddJsonFile("host.json", optional: true)
        .AddJsonFile("local.settings.json", optional: true)
        .AddEnvironmentVariables();

        var config = builder.Build();

        var keyVaultUri = config["KeyVault_Uri_Test"]!;
        var tenantId = config["KeyVault_TenantId"];
        var clientId = config["KeyVault_ClientId"];
        var clientSecret = config["KeyVault_ClientSecret"];

        builder
        .AddAzureKeyVault(new Uri(keyVaultUri),
                        new ClientSecretCredential(
                            tenantId,
                            clientId,
                            clientSecret))
        .AddJsonFile("local.settings.json", optional: true);
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddOptions<UserContextOptions>().Configure<IConfiguration>(
            (settings, configuration) => configuration.GetSection(nameof(UserContextOptions)).Bind(settings));
    })
    .Build();

await host.RunAsync();