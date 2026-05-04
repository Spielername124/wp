namespace backend.GameManagement.GameLogic;

internal static class BitBoardPreCalculation
{
    internal static readonly ulong[,] LOSBitboardArray;
    internal static readonly bool[,] IsDiagonalBitBoardArray;
    internal static readonly bool[,] IsVerticalBitBoardArray;
    internal static readonly bool[,] IsHorizontalBitBoardArray;
    //This Array DOES NOT contain the Rochade movements.
    internal static readonly ulong [] KingMovementBitBoardArray;
    internal static readonly ulong [] KnightMovementBitBoardArray;

    static BitBoardPreCalculation()
    {
        LOSBitboardArray = new ulong[64,64];
        IsDiagonalBitBoardArray = new bool[64,64];
        IsVerticalBitBoardArray = new bool[64,64];
        IsHorizontalBitBoardArray = new bool[64,64];
        KingMovementBitBoardArray = new ulong[64];
        KnightMovementBitBoardArray = new ulong[64];

        // calculate the boards for every value in the Hollow Symmetric Matrix
        for (int i = 0; i < 64; i++)
        {
            for (int j = i+1; j < 64; j++)
            {
                CalculateLineBitboardsOnIndex(i, j);
            }
            
            //Calculates the KingMovementBitboard
            CalculateKingBitboardsOnIndex(i);
            //Calculates KnightMovementBitBoard
            CalculateKnightBitboardsOnIndex(i);
            

        } 
    }

    private static void CalculateLineBitboardsOnIndex(int index1, int index2)
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

    private static void CalculateKingBitboardsOnIndex(int index)
    {
        if(index - 8 >= 0) KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 8);
        if(index + 8 < 64) KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 8);
        
        
        if(index - 9 >= 0 && DoesIndexNotRollOver(index,index-9,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 9);
        if(index - 7 >= 0 && DoesIndexNotRollOver(index,index-7,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 7);
        if(index - 1 >= 0 && DoesIndexNotRollOver(index,index-1,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 1);
        
        if(index + 1 < 64 && DoesIndexNotRollOver(index,index +1,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 1);
        if(index + 7 < 64 && DoesIndexNotRollOver(index,index + 7,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 7);
        if(index + 9 < 64 && DoesIndexNotRollOver(index,index + 9,1))
            KingMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 9);
    }
    
    private static void CalculateKnightBitboardsOnIndex(int index)
    {
        if(index - 17 >= 0 && DoesIndexNotRollOver(index,index-17,2))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 17);
        if(index - 15 >= 0 && DoesIndexNotRollOver(index,index-15,2))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 15);
        if(index - 10 >= 0 && DoesIndexNotRollOver(index,index-10,1))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 10);
        if(index - 6 >= 0 && DoesIndexNotRollOver(index,index-6,1))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 6);

        if(index + 6 < 64 && DoesIndexNotRollOver(index,index + 6,1))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 6);
        if(index + 10 < 64 && DoesIndexNotRollOver(index,index + 10,1))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 10);
        if(index + 15 < 64 && DoesIndexNotRollOver(index,index + 15,2))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 15);
        if(index + 17 < 64 && DoesIndexNotRollOver(index,index + 17,2))
            KnightMovementBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 17);
    }

    private static bool DoesIndexNotRollOver(int originIndex, int targetIndex, int expectedRowChangeOffset)
    {
        return (Math.Abs(originIndex/8 - targetIndex/8) == expectedRowChangeOffset);
    }
}