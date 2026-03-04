using System.Data;
using Dapper;

namespace backend.GameManagement;

public static class GameInfoEndpoints
{
    public static void MapGameInfoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/gameinfos");
        group.MapGet("/", GetAllGames);
        group.MapPost("/", CreateNewGame);

    }

    private static async Task<IResult> GetAllGames(IDbConnection db)
    {
        var games = await db.QueryAsync<GameInfo>(
            "SELECT GameId, PlayerId, OpponentId, GameState FROM gameinfos");
        return TypedResults.Ok(games);
    }

    private static async Task<IResult> CreateNewGame(GameInfo gameInfo, IDbConnection db)
    {
        gameInfo.GameId = await db.QuerySingleAsync<int>(
            "INSERT INTO gameinfos (playerid, opponentid, gamestate) VALUES (@PlayerID, @OpponentID, @GameState) RETURNING gameid",
            gameInfo);

        return TypedResults.Created($"/gameinfos/{gameInfo.GameId}", gameInfo);
    }
}
