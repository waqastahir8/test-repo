using AmeriCorps.Users.Data.Core;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
     .AddJsonFile("appsettings.json", optional: false)
     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
     .AddJsonFile("appsettings.local.json", optional: true);

var keyVaultUri = builder.Configuration["KeyVaultOptions:KeyVaultUri"]!;
var tenantId = builder.Configuration["KeyVaultOptions:TenantId"];
var clientId = builder.Configuration["KeyVaultOptions:ClientId"];
var clientSecret = builder.Configuration["KeyVaultOptions:ClientSecret"];

if (!string.IsNullOrEmpty(keyVaultUri) &&
    !string.IsNullOrEmpty(tenantId) &&
    !string.IsNullOrEmpty(clientId) &&
    !string.IsNullOrEmpty(clientSecret))
{
    builder.Configuration
    .AddAzureKeyVault(new Uri(keyVaultUri),
                        new ClientSecretCredential(
                            tenantId,
                            clientId,
                            clientSecret));
}

var configuration =
     builder.Configuration
     .AddJsonFile("appsettings.local.json", optional: true)
     .Build();

var connectionString = builder.Configuration["UserContextOptions:DefaultConnectionString"];

builder.Services.AddDbContext<UserDbContext>(
               options => options
               .UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", NpgsqlContext.Schema))
               .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>()
               .UseSnakeCaseNamingConvention());

var app = builder.Build();
await app.RunAsync();