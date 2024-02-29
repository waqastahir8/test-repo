using Microsoft.EntityFrameworkCore;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Data.Migrations;

public class UserDbContext : DbContext
{

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
                .ToTable("User")
                .Property(u => u.DateOfBirth)
                .HasColumnType("date");

        modelBuilder.Entity<Education>(e => {
                e.Property(p => p.DateAttendedFrom).HasColumnType("date");
                e.Property(p => p.DateAttendedTo).HasColumnType("date");
        });
    }
}