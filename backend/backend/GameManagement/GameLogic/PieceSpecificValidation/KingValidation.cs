namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal class KingValidation
{
    internal static bool ValidateKing(GameInfo gameInfo, int originField, int targetField)
    {
        return ((BitBoardPreCalculation.KingMovementBitBoardArray[originField] |
                 GeneralBitBoardHelper.BitBoardOnIndex(targetField)) != 0) 
               || IsRochade(gameInfo, originField, targetField);
    }

    private static bool IsRochade(GameInfo gameInfo, int originField, int targetField)
    {
        //TODO
        return false;
    }
}