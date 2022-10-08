using Microsoft.EntityFrameworkCore;
using WebSecForDummiesMvc.Models;

namespace WebSecForDummiesMvc.Data;

public class WalletContext : DbContext
{
    public WalletContext(DbContextOptions<WalletContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.HasIndex(p => p.Username).IsUnique();
            user.Property(p => p.RegisteredTimestamp).HasDefaultValue(DateTimeOffset.UtcNow);
        });
    }
}