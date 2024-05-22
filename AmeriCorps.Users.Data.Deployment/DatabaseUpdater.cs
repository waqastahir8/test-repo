using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Migrations;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Data.Deployment;

public class DatabaseUpdater
{
    private readonly ILogger<DatabaseUpdater> _logger;

    private readonly UserContextOptions _options;

    public DatabaseUpdater(
        IOptions<UserContextOptions> options,
        ILogger<DatabaseUpdater> logger)
    {
        _logger = logger;
        _options = options?.Value ?? new();
    }

    [Function("DatabaseUpdate")]
#pragma warning disable IDE0060 // Remove unused parameter
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest request)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        _logger.LogInformation($"C# HTTP trigger function processed to apply EF migration.");

        try
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder
                .UseNpgsql(_options.DefaultConnectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", NpgsqlContext.Schema))
                .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>();
            using var context = new UserDbContext(builder.Options);
            await context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to apply database migration");
            return new StatusCodeResult(500);
        }

        string responseMessage = "Listings database have been migrated to the latest version successfully";

        return new OkObjectResult(responseMessage);
    }
}