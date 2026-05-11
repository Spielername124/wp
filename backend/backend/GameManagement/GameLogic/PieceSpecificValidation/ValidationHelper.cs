namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal static class ValidationHelper
{
    // Checks if two indices are on a diagonal and that there are no pieces between the indices, blocking the LOS
    internal static bool IsValidDiagonalMove(GameInfo gameInfo, int originField, int targetField)
    {
        return 
            //Check the pre-calculated bool whether the indices are on a diagonal
            (BitBoardPreCalculation.IsDiagonalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
            (gameInfo.FullBitBoard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }

    internal static bool IsValidVerticalMove(GameInfo gameInfo, int originField, int targetField)
    {
        return 
            //Check the pre-calculated bool whether the indices are on a vertical
            (BitBoardPreCalculation.IsVerticalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
             (gameInfo.FullBitBoard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }
    
    internal static bool IsValidHorizontalMove(GameInfo gameInfo, int originField, int targetField)
    {
        return 
            //Check the pre-calculated bool whether the indices are on a horizontal
            (BitBoardPreCalculation.IsHorizontalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
             (gameInfo.FullBitBoard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }
    
    
}