using Microsoft.EntityFrameworkCore;
using SqlInjectionDemo.Models;

namespace SqlInjectionDemo.Data;

public class DemoContext : DbContext
{
    public DemoContext(DbContextOptions<DemoContext> options) : base(options)
    {
    }

    public DbSet<Board> Boards { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(user =>
        {
            user.Property(p => p.CreatedTimestamp).HasDefaultValue(DateTimeOffset.UtcNow);
        });
    }
}