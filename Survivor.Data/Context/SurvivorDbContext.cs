using Microsoft.EntityFrameworkCore;
using Survivor.Data.Entities;

namespace Survivor.Data.Context;

public class SurvivorDbContext : DbContext
{
    public SurvivorDbContext(DbContextOptions<SurvivorDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<CompetitorEntity> Competitors { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SurvivorDb;Username=cagdasergenc;Password=112358");
    }
}