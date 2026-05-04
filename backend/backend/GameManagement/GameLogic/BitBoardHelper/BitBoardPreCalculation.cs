namespace backend.GameManagement.GameLogic;

internal static class BitBoardPreCalculation
{
    internal static readonly ulong[,] LOSBitboardArray;
    internal static readonly bool[,] IsDiagonalBitBoardArray;
    internal static readonly bool[,] IsVerticalBitBoardArray;
    internal static readonly bool[,] IsHorizontalBitBoardArray;

    static BitBoardPreCalculation()
    {
        LOSBitboardArray = new ulong[64,64];
        IsDiagonalBitBoardArray = new bool[64,64];
        IsVerticalBitBoardArray = new bool[64,64];
        IsHorizontalBitBoardArray = new bool[64,64];

        // calculate the boards for every value in the Hollow Symmetric Matrix
        for (int i = 0; i < 64; i++)
        {
            for (int j = i+1; j < 64; j++)
            {
                CalculateBitboardsOnIndex(i, j);
            }
        } 
    }

    private static void CalculateBitboardsOnIndex(int index1, int index2)
    {
        ulong result=0;

        int lowerIndex = index1;
        int upperIndex = index2;
        
        int lowerIndexRow=lowerIndex/8;
        int lowerIndexCol = lowerIndex%8;
      
        int upperIndexRow=upperIndex/8;
        int upperIndexCol=upperIndex%8;
      
        int colDiff= Math.Abs(upperIndexCol-lowerIndexCol);
        int rowDiff=upperIndexRow-lowerIndexRow;

        int stepsBetweenIndices = 0;
        int slidingValue = 0;
        
        //Diagonal case
        if (rowDiff == colDiff)
        {
            slidingValue=lowerIndexCol < upperIndexCol ? 9 : 7;
            stepsBetweenIndices = colDiff - 1;
            IsDiagonalBitBoardArray[index1, index2] = true;
            IsDiagonalBitBoardArray[index2, index1] = true;
        }
        //vertical case
        else if (lowerIndexCol == upperIndexCol)
        {
            slidingValue = 8;
            stepsBetweenIndices = rowDiff - 1;
            IsVerticalBitBoardArray[index1, index2] = true;
            IsVerticalBitBoardArray[index2, index1] = true;
        }
        
        //Horizontal case
        else if (lowerIndexRow == upperIndexRow)
        {
            slidingValue = 1;
            stepsBetweenIndices = colDiff - 1;
            IsHorizontalBitBoardArray[index1, index2] = true;
            IsHorizontalBitBoardArray[index2, index1] = true;
        }
        ulong slider = GeneralBitBoardHelper.BitBoardOnIndex(lowerIndex);
        
        //check the fields between both indices.
        for (int currentStep = 0; currentStep < stepsBetweenIndices; currentStep++)
        {
            slider <<= slidingValue;
            result |= slider;
        }
        LOSBitboardArray [index1,index2] = result;
        LOSBitboardArray [index2, index1] = result;
    }
}