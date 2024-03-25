using FedTec.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmeriCorps.Users.Data.Core;

public abstract class NpgsqlContext : ContextBase
{
    protected NpgsqlContext(DbContextOptions options) : base(options) { }

    protected NpgsqlContext() { }

    public DbSet<User> Users { get; set; }

    public DbSet<SavedSearch> SavedSearch { get; set; }
    public static string Schema => "users";
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<User>()
                .ToTable("User")
                .Property(u => u.DateOfBirth)
                .HasColumnType("date");

        modelBuilder.Entity<Education>(e =>
        {
            e.Property(p => p.DateAttendedFrom).HasColumnType("date");
            e.Property(p => p.DateAttendedTo).HasColumnType("date");
        });


        modelBuilder.Entity<SavedSearch>(s =>
        {
            s.Property(p => p.LastRun).HasColumnType("date");
            s.Property(p => p.Name).HasMaxLength(256);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}

public sealed class RepositoryContext : NpgsqlContext
{
}