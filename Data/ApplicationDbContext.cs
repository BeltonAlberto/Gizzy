using Microsoft.EntityFrameworkCore;

namespace gizzy;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
}
