using System.Data;
using Dapper;

namespace backend.GameManagement;

public static class GameInfoEndpoints
{
    public static void MapGameInfoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/gameinfo");
        group.MapPost("/", CreateNewGame);
        group.MapGet("/", GetAllGames);
        group.MapGet("/{GameId}", GetSpecificGame);
        group.MapPut("/{GameId}", UpdateGameInfo);
        group.MapDelete("/", DeleteEverything);
        group.MapDelete("/{gameId}", DeleteGameInfo);


    }

    private static async Task<IResult> CreateNewGame(GameInfo gameInfo, IDbConnection db)
    {
        gameInfo.GameId = await db.QuerySingleAsync<int>(
            "INSERT INTO gameinfos (playerid, opponentid, gamestate) VALUES (@PlayerID, @OpponentID, @GameState) RETURNING gameid",
            gameInfo);

        return TypedResults.Created($"/gameinfos/{gameInfo.GameId}", gameInfo);
    }
    private static async Task<IResult> GetAllGames(IDbConnection db)
    {
        var games = await db.QueryAsync<GameInfo>(
            "SELECT GameId, PlayerId, OpponentId, GameState, Turn, HasTerminated FROM gameinfos");
        return TypedResults.Ok(games);
    }
    private static async Task<IResult> GetSpecificGame(int gameId, IDbConnection db)
    {
        var game  = await db.QueryFirstOrDefaultAsync<GameInfo>(
            "SELECT GameId, PlayerId, OpponentId, GameState, Turn, HasTerminated FROM gameinfos WHERE gameId = @GameId");
        return game != null ? TypedResults.Ok(game): TypedResults.NotFound();
    }
    //Should acctually not be needed (?) since the gamestate cant directly be changed by the user
    static async Task<IResult> UpdateGameInfo(int gameId, GameInfo inputGameInfo, IDbConnection db)
    {
        var rowsAffected = await db.ExecuteAsync(
            "UPDATE gameinfos SET PlayerId = @PlayerId, OpponentId = @OpponentId, GameState = @GameState, Turn = @Turn WHERE gameId = @GameId",
            new { inputGameInfo.PlayerId, inputGameInfo.OpponentId, inputGameInfo.GameState, GameId = gameId, Turn = inputGameInfo.Turn+1});

        return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    static async Task<IResult> DeleteGameInfo(int gameId, IDbConnection db)
    {
        var rowsAffected = await db.ExecuteAsync(
            "DELETE FROM gameinfos WHERE gameId =@GameId", new { GameId = gameId });

        return rowsAffected > 0 ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    // For debuging only!  TODO: !Important! Remove before deploying
    static async Task<IResult> DeleteEverything(IDbConnection db)
    {
        await db.ExecuteAsync("DELETE FROM gameinfos");
        
        return TypedResults.NoContent();
    }
}
