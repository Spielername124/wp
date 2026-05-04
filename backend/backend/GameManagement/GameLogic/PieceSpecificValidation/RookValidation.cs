namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal static class RookValidation
{
    internal static bool ValidateRook(GameInfo gameinfo, int originField, int targetField, bool color)
    {
        return
            //we already know from the general validationthat the targetField is either occupied by an enemy or is free
            // Hence we only need to check if it is a horizontal or vertical move with valid LOS 
            (ValidationHelper.IsValidVerticalMove(gameinfo, originField, targetField) ||
             ValidationHelper.IsValidHorizontalMove(gameinfo, originField, targetField));

    }
}