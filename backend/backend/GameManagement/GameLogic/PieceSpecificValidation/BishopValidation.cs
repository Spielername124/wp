namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal static class BishopValidation
{
    internal static bool ValidateBishop(GameInfo gameInfo, int originField, int targetField)
    {
        //Checks if this is a valid diagonal move
        return ValidationHelper.IsValidDiagonalMove(gameInfo, originField, targetField);
    }
    
}