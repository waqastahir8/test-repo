using Microsoft.Extensions.Hosting;
using AmeriCorps.Users.Data.Core;

var builder = Host.CreateApplicationBuilder(args);

var keyVaultUri = builder.Configuration["KeyVaultOptions:KeyVaultUri"]!;
var tenantId = builder.Configuration["KeyVaultOptions:TenantId"];
var clientId = builder.Configuration["KeyVaultOptions:ClientId"];
var clientSecret = builder.Configuration["KeyVaultOptions:ClientSecret"];

var Configuration = builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true)
                .AddAzureKeyVault(new Uri(keyVaultUri),
                     new ClientSecretCredential(
                            tenantId,
                            clientId,
                            clientSecret))
               .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

var usersDbConnectionStr = builder.Configuration["UserContextOptions:DefaultConnectionString"];

builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(usersDbConnectionStr, x => x.MigrationsHistoryTable("__EFMigrationsHistory", NpgsqlContext.Schema)));

var app = builder.Build();
await app.RunAsync();