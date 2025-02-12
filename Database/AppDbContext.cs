using Microsoft.EntityFrameworkCore;
using webapiwc.Models;

namespace webapiwc.Database;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

     }

    public DbSet<Todo> Todo { get; set; }
    
}
