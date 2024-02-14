using AmeriCorps.Users.Data.Migrations;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME") ?? 
                        throw new Exception("Environment variable KEY_VAULT_NAME is missing.");

string? kvConnectionStr = await readKeyVaultSecretAsync(keyVaultName, "UsersDBConnStr");

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true);

// Take connection str from appsettings*.json if not in key vault or no key vault access
var usersDbConnectionStr = kvConnectionStr ??
                        builder.Configuration.GetConnectionString("UsersDBConnStr");


builder.Services.AddDbContext<UserDbContext>(options =>
        options.UseNpgsql(usersDbConnectionStr));


var app = builder.Build();
app.Run();


//Helper method to re ad secret from keyvault
async Task<string> readKeyVaultSecretAsync(string kvName, string secret){
        var kvUri = "https://" + keyVaultName + ".vault.azure.net";

        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        string? kvConnectionStr = null;
        try {
                var kvConnection = await client.GetSecretAsync(secret);
                kvConnectionStr = kvConnection.Value.Value;
        } catch(Exception ex) {
                Console.WriteLine(
                        "Database connection string UsersDBConnStr not found on Azure Keyvault {0}. Error: {1}", 
                        keyVaultName,ex.Message);
        }
        return kvConnectionStr;
}