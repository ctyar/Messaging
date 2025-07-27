using Microsoft.EntityFrameworkCore;

namespace Messaging;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source=Database.db");
}

public sealed class Order
{
    public int Id { get; set; }
}
