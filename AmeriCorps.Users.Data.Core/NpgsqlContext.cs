using AmeriCorps.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmeriCorps.Users.Data.Core;

public abstract class NpgsqlContext : ContextBase
{
    protected NpgsqlContext(DbContextOptions options) : base(options) { }

    protected NpgsqlContext() { }

    public DbSet<User> Users { get; set; }

    public DbSet<SavedSearch> SavedSearch { get; set; }

    public DbSet<Collection> Collection { get; set; }
    public static string Schema => "users";
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);

        var address = Create<Address>("address");

        var attribute = Create<Attribute>("attribute");

        var communicationMethod = Create<CommunicationMethod>("communication_method");

        var education = Create<Education>("education");
        education.Property(p => p.DateAttendedFrom).HasColumnType("date");
        education.Property(p => p.DateAttendedTo).HasColumnType("date");

        var language = Create<Language>("language");

        var militaryService = Create<MilitaryService>("military_service");

        var reference = Create<Reference>("reference");
        reference.Property(s => s.DateContacted).HasColumnType("date");

        var relative = Create<Relative>("relative");

        var savedSearch = Create<SavedSearch>("saved_search");
        savedSearch.Property(p => p.LastRun).HasColumnType("date");
        savedSearch.Property(p => p.LastRun).HasColumnType("date");
        savedSearch.Property(p => p.Name).HasMaxLength(256);

        var skill = Create<Skill>("skill");

        var user = Create<User>("user");
        user.Property(p => p.DateOfBirth).HasColumnType("date");

        var collection = Create<Collection>("collection");


        EntityTypeBuilder<T> Create<T>(string tableName) where T : Entity
        {
            var entity = modelBuilder.Entity<T>();

            entity.ToTable(tableName);
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            return entity;
        }

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(ConnectionString)
                .UseSnakeCaseNamingConvention();
        }
    }
}

public sealed class RepositoryContext : NpgsqlContext
{
}