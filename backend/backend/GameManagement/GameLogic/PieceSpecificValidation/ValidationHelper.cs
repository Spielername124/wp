namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal class ValidationHelper
{
    // Checks if two indices are on a diagonal and that there are no pieces between the indices, blocking the LOS
    internal static bool ValidateDiagonalLineOfSight(GameInfo gameInfo, int fieldOfOrigin, int targetField)
    { 
        /*
         We convert both indices into a lower and a higher index to simplify the sliding by only ever needing
         to slide from the lower index to the higher. That works since we don't care what actually is on the indices,
         but only if there is something between the indices, restricting LOS
         */
        
        int lowerIndex=Math.Min(fieldOfOrigin, targetField);
        int upperIndex = Math.Max(fieldOfOrigin, targetField);
        
        int lowerIndexRow=lowerIndex/8;
        int lowerIndexCol = lowerIndex%8;
      
        int upperIndexRow=upperIndex/8;
        int upperIndexCol=upperIndex%8;
      
        int colDiff= Math.Abs(upperIndexCol-lowerIndexCol);
        int rowDiff=upperIndexRow-lowerIndexRow;

        //rejecting the moves that are not at a diagonal
        if (rowDiff != colDiff) return false;
        
        //if we need to slide upside left, we need to shift by 7 bits, if upside right, we shift 9 bits.
        int slidingValue=lowerIndexCol < upperIndexCol ? 9 : 7;

        ulong fullBitboard = BitBoardHelper.GetFullBoard(gameInfo);
        ulong slider = BitBoardHelper.BitBoardOnIndex(lowerIndex);
        
        //check the fields between both indices.
        for (int currentStep = 0; currentStep < colDiff -1; currentStep++)
        {
            slider <<= slidingValue;
            if ((slider & fullBitboard) != 0) return false;
        }
        // This should work, but we can do this in O(1) by precomputing the bitboard of the combined sliding
        // TODO --> Optimise this by precomputing the whole thing.
      
        
        return true;
    }

    internal static bool ValidateVerticalLineOfSight(GameInfo gameInfo, int fieldOfOrigin, int targetField)
    {
        //TODO
        return true;
    }
    
    
}