namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal static class KnightVailidation
{
    internal static bool ValidateKnight(GameInfo gameInfo, int originField, int targetField)
    {
        return ((BitBoardPreCalculation.KnightMovementBitBoardArray[originField] &
                 GeneralBitBoardHelper.BitBoardOnIndex(targetField)) != 0);
    }
}