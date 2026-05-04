namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal class ValidationHelper
{
    // Checks if two indices are on a diagonal and that there are no pieces between the indices, blocking the LOS
    internal static bool ValidateDiagonalLineOfSight(GameInfo gameInfo, int originField, int targetField)
    {
        //Get friend and foe pieces
        ulong fullBitboard = GeneralBitBoardHelper.GetFullBoard(gameInfo);
        
        return 
            //Check the pre-calculated bool whether the indices are on a diagonal
            (BitBoardPreCalculation.IsDiagonalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
            (fullBitboard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }

    internal static bool ValidateVerticalLineOfSight(GameInfo gameInfo, int originField, int targetField)
    {
        //Get friend and foe pieces
        ulong fullBitboard = GeneralBitBoardHelper.GetFullBoard(gameInfo);
        
        return 
            //Check the pre-calculated bool whether the indices are on a vertical
            (BitBoardPreCalculation.IsVerticalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
             (fullBitboard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }
    
    internal static bool ValidateHorizontalLineOfSight(GameInfo gameInfo, int originField, int targetField)
    {
        //Get friend and foe pieces
        ulong fullBitboard = GeneralBitBoardHelper.GetFullBoard(gameInfo);
        
        return 
            //Check the pre-calculated bool whether the indices are on a horizontal
            (BitBoardPreCalculation.IsHorizontalBitBoardArray[originField, targetField] &&
             // checks with the precalculated values for the LOS if the LOS exists
             (fullBitboard & BitBoardPreCalculation.LOSBitboardArray[originField,targetField])==0);
    }
    
    
}