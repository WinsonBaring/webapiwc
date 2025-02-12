using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using webapiwc.Database;
using webapiwc.Models;
namespace webapiwc.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo");

        group.MapGet("/", async (AppDbContext db) => 
            await db.Todo.ToListAsync());

        group.MapGet("/{id}", async (int id, AppDbContext db) => 
            await db.Todo.FindAsync(id)
                is Todo todo
                    ? Results.Ok(todo)
                    : Results.NotFound());

        group.MapPost("/", async (Todo todo, AppDbContext db) => {
            await db.Todo.AddAsync(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/api/todo/{todo.Id}", todo);
        });

        group.MapPut("/{id}", async (int id, Todo todo, AppDbContext db) => {
            var todoToUpdate = await db.Todo.FindAsync(id);
            if (todoToUpdate == null)
                return Results.NotFound();
            todoToUpdate.Title = todo.Title;
            todoToUpdate.IsDone = todo.IsDone;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) => {
            var todoToDelete = await db.Todo.FindAsync(id);
            if (todoToDelete == null)
                return Results.NotFound();
            db.Todo.Remove(todoToDelete);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}