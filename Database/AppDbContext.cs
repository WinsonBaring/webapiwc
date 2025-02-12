using Microsoft.EntityFrameworkCore;
using webapiwc.Models;

namespace webapiwc.Database;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

     }

    public DbSet<TodoModel> Todo { get; set; }
    public DbSet<UserModel> User { get; set; }
    
}
