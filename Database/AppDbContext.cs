using Microsoft.EntityFrameworkCore;
using webapiwc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace webapiwc.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<TodoModel> Todo => Set<TodoModel>();
    public DbSet<UserModel> User => Set<UserModel>();
    public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TodoModel>()
            .HasOne(t => t.User)       // TodoModel has one User
            .WithMany(u => u.Todos)    // UserModel has many Todos
            .HasForeignKey(t => t.UserId) // Foreign key in TodoModel
            .OnDelete(DeleteBehavior.Cascade); // Delete Todos if User is deleted

        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.Todos)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Todos)
            .WithOne(t => t.ApplicationUser)
            .HasForeignKey(t => t.applicationUserId);
    }
}
