using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
        base.Database.EnsureCreated();
    }
    
    public required DbSet<FeatureFlag> FeatureFlags { get; set; }
}
