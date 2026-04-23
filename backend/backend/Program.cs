using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

// 3. Datenbank-Migration mit DbUp
var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

// Stellt sicher, dass die Datenbank auf dem Server existiert
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

app.Run();