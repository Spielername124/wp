namespace backend.GameManagement.GameLogic;

internal static class BitBoardPreCalculation
{
    internal static readonly ulong[,] LOSBitboardArray;
    internal static readonly bool[,] IsDiagonalBitBoardArray;
    internal static readonly bool[,] IsVerticalBitBoardArray;
    internal static readonly bool[,] IsHorizontalBitBoardArray;
    //This Array DOES NOT contain the Rochade movements.
    internal static readonly ulong [] SurroundingBitBoardArray;
    internal static readonly ulong [] KnightMovementBitBoardArray;
    /*[KingPosition][direction],
     /*Direction = 0 - 7, 0 = north, 1 = south, 2 = east, 3 = west, 4 = northeast,
     5 = southwest, 6 = southeast, 7= northwest, 8 = straight line, 9 = diagonal line*/
    //This is made as Jagged Array to allow more efficient manual caching.
    internal static readonly ulong[][] CheckThreatLineArray;
    internal static readonly ulong[] ThreadByWhitePawnsArray;
    internal static readonly ulong[] ThreadByBlackPawnsArray;
    internal static readonly ulong[] ThreadByKnightArray;

    static BitBoardPreCalculation()
    {
        LOSBitboardArray = new ulong[64,64];
        IsDiagonalBitBoardArray = new bool[64,64];
        IsVerticalBitBoardArray = new bool[64,64];
        IsHorizontalBitBoardArray = new bool[64,64];
        SurroundingBitBoardArray = new ulong[64];
        KnightMovementBitBoardArray = new ulong[64];
        CheckThreatLineArray = new ulong[64][];
        for (int i = 0; i < 64; i++)
        {
            CheckThreatLineArray[i]=new ulong[8];
        }
        ThreadByWhitePawnsArray= new ulong[64];
        ThreadByBlackPawnsArray = new  ulong[64];
        ThreadByKnightArray = new ulong[64];

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
            //Calculates PawnThreats
            CalculatePawnThreats(i);
            //Calculates the ThreadLines on a given index
            CalculateCheckThreatLines(i);
            //Calculates bitboards of possible Threatening Knights
            CalculateKnightThreat(i);
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
        if(index - 8 >= 0) SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 8);
        if(index + 8 < 64) SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 8);
        
        
        if(index - 9 >= 0 && DoesIndexNotRollOver(index,index-9,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 9);
        if(index - 7 >= 0 && DoesIndexNotRollOver(index,index-7,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 7);
        if(index - 1 >= 0 && DoesIndexNotRollOver(index,index-1,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 1);
        
        if(index + 1 < 64 && DoesIndexNotRollOver(index,index +1,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 1);
        if(index + 7 < 64 && DoesIndexNotRollOver(index,index + 7,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 7);
        if(index + 9 < 64 && DoesIndexNotRollOver(index,index + 9,1))
            SurroundingBitBoardArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 9);
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

    private static void CalculateCheckThreatLines(int index)
    {
        int row = index / 8;
        int colum = index % 8;
        
        int possibleNorthEast = 7-Math.Max(row, colum);
        int possibleSouthWest = 7-Math.Max(7-row, 7-colum);
        
        int possibleSouthEast = 7 - Math.Max(7-row, colum);
        int possibleNorthWest = 7 - Math.Max(row, 7-colum);
        
        
        CheckThreatLineArray[index][0] = CalculateDirection(index, 8, true, 7-row);
        CheckThreatLineArray[index][1] = CalculateDirection(index, 8, false, row);
        
        CheckThreatLineArray[index][2] = CalculateDirection(index, 1, true, 7-colum);
        CheckThreatLineArray[index][3] = CalculateDirection(index, 1, false, colum);
        
        CheckThreatLineArray[index][4] = CalculateDirection(index, 9, true, possibleNorthEast);
        CheckThreatLineArray[index][5] = CalculateDirection(index, 9, false, possibleSouthWest);
        
        CheckThreatLineArray[index][6] = CalculateDirection(index, 7, false, possibleSouthEast);
        CheckThreatLineArray[index][7] = CalculateDirection(index, 7, true, possibleNorthWest);
        
        CheckThreatLineArray[index][8] = CheckThreatLineArray[index][0] | CheckThreatLineArray[index][1] |
                                         CheckThreatLineArray[index][2] | CheckThreatLineArray[index][3];
        CheckThreatLineArray[index][9] = CheckThreatLineArray[index][4] | CheckThreatLineArray[index][5]|
                                         CheckThreatLineArray[index][6] | CheckThreatLineArray[index][7];
            ;
    }

    private static ulong CalculateDirection(int index, int slidingValue, bool direction, int slideAmount)
    {
        ulong result = 0;
        ulong slider = GeneralBitBoardHelper.BitBoardOnIndex(index);
        for (int i = 0; i < slideAmount; i++)
        {
            if (direction) 
                slider <<= slidingValue;
            else 
                slider >>= slidingValue;

            result |= slider;
        }
        return result;
    }

    private static void CalculatePawnThreats(int index)
    {
        ulong bResult = 0;
        if (DoesIndexNotRollOver(index, index + 7, 1))
            bResult |= GeneralBitBoardHelper.BitBoardOnIndex(index + 7);
        if(DoesIndexNotRollOver(index, index + 9, 1))
            bResult |= GeneralBitBoardHelper.BitBoardOnIndex(index + 9);
        ThreadByBlackPawnsArray[index] =bResult;
        
        ulong wResult = 0;
        if (DoesIndexNotRollOver(index, index - 7, 1))
            wResult |= GeneralBitBoardHelper.BitBoardOnIndex(index - 7);
        if(DoesIndexNotRollOver(index, index - 9, 1))
            wResult |= GeneralBitBoardHelper.BitBoardOnIndex(index - 9);
        ThreadByWhitePawnsArray[index] =wResult;
    }

    private static void CalculateKnightThreat(int index)
    {
        if (index + 17 < 64 && DoesIndexNotRollOver(index, index + 17, 2))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 17);
        if (index + 15 < 64 && DoesIndexNotRollOver(index, index + 15, 2))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 15);
        if (index - 15 >= 0 && DoesIndexNotRollOver(index, index - 15, 2))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 15);
        if (index - 17 >= 0 && DoesIndexNotRollOver(index, index - 17, 2))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 17);
        // Knight Moves: 1 row 2 columns (Offsets 10, 6)
        if (index + 10 < 64 && DoesIndexNotRollOver(index, index + 10, 1))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 10);
        if (index + 6 < 64 && DoesIndexNotRollOver(index, index + 6, 1))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index + 6);
        if (index - 6 >= 0 && DoesIndexNotRollOver(index, index - 6, 1))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 6);
        if (index - 10 >= 0 && DoesIndexNotRollOver(index, index - 10, 1))
            ThreadByKnightArray[index] |= GeneralBitBoardHelper.BitBoardOnIndex(index - 10);
    }
}