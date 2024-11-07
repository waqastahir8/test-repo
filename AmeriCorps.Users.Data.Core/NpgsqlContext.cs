using AmeriCorps.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmeriCorps.Users.Data.Core;

public abstract class NpgsqlContext : ContextBase
{
    protected NpgsqlContext(DbContextOptions options) : base(options)
    {
    }

    protected NpgsqlContext()
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<SavedSearch> SavedSearch { get; set; }

    public DbSet<Collection> Collection { get; set; }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<UserProject> UserProjects { get; set; }

    public DbSet<Access> Access { get; set; }

    public DbSet<OperatingSite> OperatingSites { get; set; }

    public DbSet<SocialSecurityVerification> SocialSecurityVerification { get; set; }

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

        var role = Create<Role>("role");

        var organization = Create<Organization>("organization");

        var project = Create<Project>("project");

        var userProject = Create<UserProject>("userProject");

        var access = Create<Access>("access");

        var collection = Create<Collection>("collection");

        var operatingSite = Create<OperatingSite>("operatingSite");

        var socialSecurityVerification = Create<SocialSecurityVerification>("socialSecurityVerification");

        modelBuilder.Entity<Project>()
            .HasIndex(p => new
            {
                p.ProjectName,
                p.ProjectOrgCode,
                p.ProjectCode,
                p.ProjectId,
                p.GspProjectId,
                p.ProgramName,
                p.StreetAddress,
                p.City,
                p.State,
                p.ProjectType,
                p.Description
            })
            .HasMethod("GIST")
            .IsTsVectorExpressionIndex("english");

        modelBuilder.Entity<OperatingSite>()
            .HasIndex(o => new
            {
                o.ProgramYear,
                o.OperatingSiteName,
                o.ContactName,
                o.EmailAddress,
                o.PhoneNumber,
                o.StreetAddress,
                o.StreetAddress2,
                o.City,
                o.State,
                o.ZipCode
            })
            .HasMethod("GIST")
            .IsTsVectorExpressionIndex("english");

        modelBuilder.Entity<Award>()
            .HasIndex(a => new { a.AwardCode, a.AwardName, a.GspListingNumber })
            .HasMethod("GIST")
            .IsTsVectorExpressionIndex("english");

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