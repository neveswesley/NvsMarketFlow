using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}