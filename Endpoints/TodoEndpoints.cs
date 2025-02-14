using Microsoft.EntityFrameworkCore;
using webapiwc.Database;
using webapiwc.Models;
namespace webapiwc.Endpoints;

public static class TodoEndpoints
{
    public static RouteGroupBuilder MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo");

        group.MapPost("/", async (TodoModel todo, AppDbContext db) => {
            await db.Todo.AddAsync(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/api/todo/{todo.Id}", todo);
        });

        group.MapPut("/{id}", async (int id, TodoModel todo, AppDbContext db) => {
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