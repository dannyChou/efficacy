using Microsoft.EntityFrameworkCore;

namespace newsapi.EFs;

public class TestDbContext: DbContext
{
    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<News1> News1 { get; set; }
    public DbSet<News2> News2 { get; set; }
}
