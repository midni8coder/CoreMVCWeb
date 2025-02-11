using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CoreMinimalAPI.Todo
{
    public static class TodoApi
    {
        public static void MapRoutes(WebApplication app)
        {
            var todoGroup = app.MapGroup("/todoitems");


            todoGroup.MapGet("/", async (TodoDb db) =>
                await db.Todos.ToListAsync());

            todoGroup.MapGet("/complete", async (TodoDb db) =>
                await db.Todos.Where(t => t.IsComplete).ToListAsync());

            todoGroup.MapGet("/{id}", async (int id, TodoDb db) =>
                await db.Todos.FindAsync(id)
                    is Todo todo
                        ? Results.Ok(todo)
                        : Results.NotFound());

            todoGroup.MapPost("/", async (Todo todo, TodoDb db) =>
            {
                db.Todos.Add(todo);
                await db.SaveChangesAsync();

                return Results.Created($"/todoitems/{todo.Id}", todo);
            });

            todoGroup.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
            {
                var todo = await db.Todos.FindAsync(id);

                if (todo is null) return Results.NotFound();

                todo.Name = inputTodo.Name;
                todo.IsComplete = inputTodo.IsComplete;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            todoGroup.MapDelete("/{id}", async (int id, TodoDb db) =>
            {
                if (await db.Todos.FindAsync(id) is Todo todo)
                {
                    db.Todos.Remove(todo);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });
        }
    }
}
