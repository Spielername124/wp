using System.Data;
using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using backend.GameManagement;
using Npgsql;


DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5432")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Migration error: {result.Error}");
    Console.ResetColor();
    return; 
}

Console.WriteLine("Database is up to date.");

app.UseCors();

app.MapGameInfoEndpoints();
app.MapMoveEndpoints();

app.Run();