using FCG_Games.Domain.Entity;
using FCG_Games.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new GameMap());
    }
}
