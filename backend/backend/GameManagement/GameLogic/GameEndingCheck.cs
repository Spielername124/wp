namespace backend.GameManagement.GameLogic;

public static class GameEndingCheck
{
    internal static int CheckGameEnding(GameInfo gameState, bool enemyColor)
    {
        var checkContext=
            CalculateCheckContext.PerformCheckContextCheck(gameState, enemyColor);
        return 0;
    }
}