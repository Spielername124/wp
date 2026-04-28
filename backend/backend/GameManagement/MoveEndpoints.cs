using System.Data;
using Dapper;
namespace backend.GameManagement;

public static class MoveEndpoints
{
    public static void MapMoveEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/game");
        group.MapPost("/submit", ValidateAndCreateMove);
        group.MapGet("/{requestedGame}/history", GetHistory);
        group.MapGet("/{requestedGame}/last", GetLast);
    }

    private static void ValidateAndCreateMove()
    {
    }

    
    private static async Task<IResult> GetHistory(int requestedGame, IDbConnection db)
    {
        var game  = await db.QueryFirstOrDefaultAsync<Move>(
            "SELECT * FROM moves WHERE game_id = @GameId ORDER BY move_id",new { GameId = requestedGame });
        return game != null ? TypedResults.Ok(game): TypedResults.NotFound();
    }

    private static async Task<IResult> GetLast(int requestedGame, IDbConnection db)
    {
        var game = await db.QueryFirstOrDefaultAsync<Move>(
            "SELECT origin_field, targeted_field FROM moves WHERE game_id = @GameID ORDER BY move_id DESC LIMIT 1",
            new { GameId = requestedGame });
        return game != null ? TypedResults.Ok(game): TypedResults.NotFound();
    }
}