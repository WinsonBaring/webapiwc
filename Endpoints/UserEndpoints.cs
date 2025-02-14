using Microsoft.EntityFrameworkCore;
using webapiwc.Database;
using webapiwc.Models;
namespace webapiwc.Endpoints;

public static class UserEndpoints{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app){
        var userGroup = app.MapGroup("/api/user");
        // logger
        var logger = app.Logger;
        // get all users 
        userGroup.MapGet("/", async (AppDbContext db) => await db.User.ToListAsync());

        // get a user by id
        userGroup.MapGet("/{id}", async (Guid id, AppDbContext db) => await db.User.FindAsync(id));
        
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
        userGroup.MapPost("/{userId}/todos", async (Guid userId, TodoModel todo, AppDbContext db, ILogger<WebApplication> logger) => {
            logger.LogInformation("Creating a todo for user {UserId}", userId);
            // CHECK IF USER EXISTS
            var userExist = await db.User.FindAsync(userId);
            if (userExist == null) return Results.NotFound();

            if(string.IsNullOrEmpty(todo.Title)){
                return Results.BadRequest("Title is required");
            }
            if(string.IsNullOrEmpty(todo.Description)){
                return Results.BadRequest("Description is required");
            }
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

        // get all completed todos for a user
        userGroup.MapGet("/{userId}/todos/completed", async (Guid userId, AppDbContext db) => {
            var completedTodos = await db.Todo.Where(t => t.UserId == userId && t.IsDone).ToListAsync();
            return completedTodos.Count > 0 ? Results.Ok(completedTodos) : Results.NotFound();
        });

        // get all uncompleted todos for a user
        userGroup.MapGet("/{userId}/todos/uncompleted", async (Guid userId, AppDbContext db) => {
            var uncompletedTodos = await db.Todo.Where(t => t.UserId == userId && !t.IsDone).ToListAsync();
            return uncompletedTodos.Count > 0 ? Results.Ok(uncompletedTodos) : Results.NotFound();
        });


        return userGroup;
        
    }
}