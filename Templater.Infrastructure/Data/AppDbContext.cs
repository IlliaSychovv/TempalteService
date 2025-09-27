using Microsoft.EntityFrameworkCore;
using Templater.Domain.Entities;

namespace Templater.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }
    
    public DbSet<Template> Templates { get; set; }
}