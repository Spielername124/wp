using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));

var app = builder.Build();

app.UseCors();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();


// --- Handler-Methoden im kompakten Dapper-Stil ---

static async Task<IResult> GetAllTodos(IDbConnection db)
{
    return TypedResults.Ok(
        await db.QueryAsync<Todo>("SELECT id, name, iscomplete FROM todos"));
}

static async Task<IResult> GetCompleteTodos(IDbConnection db)
{
    return TypedResults.Ok(
        await db.QueryAsync<Todo>("SELECT id, name, iscomplete FROM todos WHERE iscomplete = true"));
}

static async Task<IResult> GetTodo(int id, IDbConnection db)
{
    var todo = await db.QuerySingleOrDefaultAsync<Todo>(
        "SELECT id, name, iscomplete FROM todos WHERE id = @Id", new { Id = id });

    return todo is not null ? TypedResults.Ok(todo) : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(Todo todo, IDbConnection db)
{
    todo.Id = await db.QuerySingleAsync<int>(
        "INSERT INTO todos (name, iscomplete) VALUES (@Name, @IsComplete) RETURNING id", todo);

    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

static async Task<IResult> UpdateTodo(int id, Todo inputTodo, IDbConnection db)
{
    var rowsAffected = await db.ExecuteAsync(
        "UPDATE todos SET name = @Name, iscomplete = @IsComplete WHERE id = @Id",
        new { inputTodo.Name, inputTodo.IsComplete, Id = id });

    return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
}

static async Task<IResult> DeleteTodo(int id, IDbConnection db)
{
    var rowsAffected = await db.ExecuteAsync(
        "DELETE FROM todos WHERE id =@.Id", new { Id = id });

    return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
}