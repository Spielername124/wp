using System.Data;
using backend.GameManagement.GameLogic;
using Dapper;
namespace backend.GameManagement;

public static class MoveEndpoints
{
    //TODO Frontent needs to convert to little endian for communications
    public static void MapMoveEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/game");
        group.MapPost("/submit", ValidateAndCreateMove);
        group.MapGet("/{requestedGame}/history", GetHistory);
        group.MapGet("/{requestedGame}/last", GetLast);
    }

    private static async Task<IResult> ValidateAndCreateMove(Move move, IDbConnection db)
    {
        var gameState = await db.QueryFirstOrDefaultAsync<backend.GameManagement.GameInfo>(
            "SELECT * FROM game_info WHERE game_id = @GameId", 
            new { GameId = move.GameId });
        if (gameState == null) return TypedResults.NotFound();
        if (MoveValidation.ValidateMove(move, gameState))
        {
            
            //TODO: write the gamestate to the db
            //TODO: Check if the game ends (Check mate/ Remis) and give acording response (including draw by repeated moves)
            return TypedResults.Ok();
        }
        
        //reject move if invalid
        return TypedResults.UnprocessableEntity();
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