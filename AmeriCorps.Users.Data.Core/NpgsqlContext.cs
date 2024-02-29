using FedTec.Data;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

public abstract class NpgsqlContext : ContextBase
{
    protected NpgsqlContext(DbContextOptions options) : base(options)
    {
    }

    protected NpgsqlContext() { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);

        //Needed because otherwise EF attemtps looking for Users
        modelBuilder.Entity<User>().ToTable("User");  
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
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