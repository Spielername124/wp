namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal class QueenValidation
{
    internal static bool ValidateQueen(GameInfo gameInfo, int originField, int targetField)
    {
        return
            //Checks if LOS exists and if it is a valid diagonal, horizontal or vertical move
            (ValidationHelper.IsValidDiagonalMove(gameInfo, originField, targetField) ||
             ValidationHelper.IsValidVerticalMove(gameInfo, targetField, originField) ||
             ValidationHelper.IsValidHorizontalMove(gameInfo, targetField, originField));
    }
    
}