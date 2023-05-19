using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MinIOExample.Application.Models;

namespace MinIOExample.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<FileMetadata> Metadata { get; set; }
        
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}