using Microsoft.EntityFrameworkCore;
using webapiwc.Database;
using webapiwc.Models;
namespace webapiwc.Endpoints;

public static class UserEndpoints{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app){
        var userGroup = app.MapGroup("/api/user");

        userGroup.MapGet("/{id}", async (Guid id, AppDbContext db) => await db.User.FindAsync(id));//
        
        userGroup.MapPost("/",async (UserModel user, AppDbContext db)=>{
            await db.User.AddAsync(user);
            await db.SaveChangesAsync();
            return Results.Created($"/api/user/{user.Id}", user);
        });
        userGroup.MapPut("/{id}", async (Guid id, UserModel user, AppDbContext db) => {
            var userToUpdate = await db.User.FindAsync(id);
            if (userToUpdate == null) return Results.NotFound();
            userToUpdate.Name = user.Name;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
        userGroup.MapDelete("/{id}", async (Guid id, AppDbContext db) => {
            var userToDelete = await db.User.FindAsync(id);
            if (userToDelete == null) return Results.NotFound();
            db.User.Remove(userToDelete);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
        // create a todo for a user
        userGroup.MapPost("/{userId}/todos", async (Guid userId, TodoModel todo, AppDbContext db) => {
            todo.UserId = userId;
            await db.Todo.AddAsync(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/api/user/{userId}/todos/{todo.Id}", todo);
        });

        // get all todos for a user
        userGroup.MapGet("/{userId}/todos", async (Guid userId, AppDbContext db) => {
            var userTodos = await db.Todo.Where(t=>t.UserId == userId).ToListAsync();
            return userTodos.Count > 0 ? Results.Ok(userTodos) : Results.NotFound();
        });

        return userGroup;
        
    }
}