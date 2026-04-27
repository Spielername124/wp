using System.Data;
using Dapper;

namespace backend.GameManagement;

/* I know Quite a few of these Endpoints wont be usefull at all,
 but I am implementing them temporarely for learnings sake and for reference
*/

public static class GameInfoEndpoints
{
    public static void MapGameInfoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/gameinfo");
        group.MapPost("/", CreateNewGame);
        group.MapGet("/", GetAllGames);
        group.MapGet("/{requestedGame}", GetSpecificGame);
        group.MapPut("/{requestedGame}", UpdateGameInfo);
        group.MapDelete("/", DeleteEverything);
        group.MapDelete("/{gameToDelete}", DeleteGameInfo);
        
    }
    
    private static async Task<IResult> CreateNewGame(GameInfo.GameInfo gameInfo, IDbConnection db)
    {
        gameInfo.GameId = await db.QuerySingleAsync<int>(
            @"INSERT INTO game_info (player_1_id, player_2_id) 
      VALUES (@Player1Id, @Player2Id) 
      RETURNING game_id",
            gameInfo);

        return TypedResults.Created($"/gameinfos/{gameInfo.GameId}", gameInfo);
    }
    
    private static async Task<IResult> GetAllGames(IDbConnection db)
    {
        var games = await db.QueryAsync<GameInfo.GameInfo>(
            "SELECT * FROM game_info");
        return TypedResults.Ok(games);
    }
    
    private static async Task<IResult> GetSpecificGame(int requestedGame, IDbConnection db)
    {
        var game  = await db.QueryFirstOrDefaultAsync<GameInfo.GameInfo>(
            "SELECT * FROM game_info WHERE game_id = @GameId",new { GameId = requestedGame });
        return game != null ? TypedResults.Ok(game): TypedResults.NotFound();
    }
    static async Task<IResult> UpdateGameInfo(int requestedGame, GameInfo.GameInfo inputGameInfo, IDbConnection db)
    {
        var rowsAffected = await db.ExecuteAsync(
            "UPDATE game_info SET turn_counter = turn_counter+1 , w_pawn = @WPawn, w_knight = @WKnight, w_bishop = @WBishop, w_rook = @WRook, w_queen = @WQueen, w_king = @WKing, b_pawn = @BPawn, b_knight = @BKnight, b_bishop = @BBishop, b_rook = @BRook, b_queen = @BQueen, b_king = @BKing WHERE game_id = @GameId",
            
            new { 
                GameId = requestedGame,
                inputGameInfo.WPawn, 
                inputGameInfo.WKnight, 
                inputGameInfo.WBishop, 
                inputGameInfo.WRook, 
                inputGameInfo.WQueen, 
                inputGameInfo.WKing, 
                inputGameInfo.BPawn, 
                inputGameInfo.BKnight, 
                inputGameInfo.BBishop, 
                inputGameInfo.BRook, 
                inputGameInfo.BQueen, 
                inputGameInfo.BKing, 
            });

        return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    
    static async Task<IResult> DeleteGameInfo(int gameToDelete, IDbConnection db)
    {
        var rowsAffected = await db.ExecuteAsync(
            "DELETE FROM game_info WHERE game_id =@GameId", new { GameId = gameToDelete });

        return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    static async Task<IResult> DeleteEverything(IDbConnection db)
    {
        await db.ExecuteAsync("DELETE FROM game_info");
        
        return TypedResults.NoContent();
    }
    
    
}