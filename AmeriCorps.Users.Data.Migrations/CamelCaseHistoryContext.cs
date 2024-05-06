using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace AmeriCorps.Users.Data.Migrations;
#pragma warning disable EF1001 // Internal EF Core API usage.   
public sealed class CamelCaseHistoryContext : NpgsqlHistoryRepository
{
    public CamelCaseHistoryContext(HistoryRepositoryDependencies dependencies) : base(dependencies)
#pragma warning restore EF1001 // Internal EF Core API usage.
    {
    }

    protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
    {
        base.ConfigureTable(history);

        history.Property(h => h.MigrationId).HasColumnName("MigrationId");
        history.Property(h => h.ProductVersion).HasColumnName("ProductVersion");
    }
}
