using Microsoft.EntityFrameworkCore;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Data.Migrations;

public class UserDbContext : DbContext
{

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().OwnsOne(up => up.BasicInfo,
        ba => {
            ba.Property(p => p.UserName).HasColumnName("UserName");
            ba.Property(p => p.Prefix).HasColumnName("Prefix");
            ba.Property(p => p.FirstName).HasColumnName("FirstName");
            ba.Property(p => p.LastName).HasColumnName("LastName");
            ba.Property(p => p.MiddleName).HasColumnName("MiddleName");
            ba.Property(p => p.PreferredName).HasColumnName("PreferredName");
            ba.Property(p => p.DateOfBirth).HasColumnName("DateOfBirth");
        });
        
        modelBuilder.Entity<User>().OwnsOne(up => up.About,
        a => {
            a.Property(p => p.Gender).HasColumnName("Gender");
             a.Property(p => p.Ethnicity).HasColumnName("Ethnicity");
             a.Property(p => p.Race).HasColumnName("Race");
             a.Property(p => p.CitizenshipStatus).HasColumnName("CitizenshipStatus");
             a.Property(p => p.FamilyCombinedIncome).HasColumnName("FamilyCombinedIncome");
             a.Property(p => p.UnexpectedExpenseConfidence).HasColumnName("UnexpectedExpenseConfidence");
             a.Property(p => p.HasValidGovtDriversLicense).HasColumnName("HasValidGovtDriversLicense");
             a.Property(p => p.VeteranStatus).HasColumnName("VeteranStatus");
        });

        

        base.OnModelCreating(modelBuilder);
    }
}
