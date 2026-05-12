namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal class KingValidation
{
    internal static bool ValidateKing(GameInfo gameInfo, int originField, int targetField,bool color)
    {
        return ((BitBoardPreCalculation.SurroundingBitBoardArray[originField] &
                 GeneralBitBoardHelper.BitBoardOnIndex(targetField)) != 0) 
               || IsCastling(gameInfo, originField, targetField, color);
    }

    private static bool IsCastling(GameInfo gameInfo, int originField, int targetField,  bool color)
    {
        if (CheckCheck.PerformCheckCheck(gameInfo, color)) return false;
        return (color, originField, targetField) switch
        { 
            /*
             In every case
             we check if the LOS exists by XOR-ing the Board with the Precalculated LOS. It is 0 if the LOS is existing
             we check if the Rook needed for the castling is on his position. If it is the Board is 0
             
             the OR-ing between these two board prevents us from checking both boards if they are 0 and then logically OR-ing this.
             */
            
            (true, 4, 2) => 
                ((gameInfo.HasNotMoved ^ GeneralBitBoardHelper.BitBoardOnIndex(0)) |
               (gameInfo.FullBitBoard & BitBoardPreCalculation.LOSBitboardArray[4,0]))==0,
            (true, 4, 6) => 
                ((gameInfo.HasNotMoved & GeneralBitBoardHelper.BitBoardOnIndex(7)) |
                (gameInfo.FullBitBoard& BitBoardPreCalculation.LOSBitboardArray[4,7]))==0,
            (false, 60, 58) => 
                ((gameInfo.HasNotMoved & GeneralBitBoardHelper.BitBoardOnIndex(56)) |
                 (gameInfo.FullBitBoard& BitBoardPreCalculation.LOSBitboardArray[60,56]))==0,
            (false, 60, 62) => 
                ((gameInfo.HasNotMoved & GeneralBitBoardHelper.BitBoardOnIndex(63)) |
                (gameInfo.FullBitBoard& BitBoardPreCalculation.LOSBitboardArray[60,63]))==0,
            _ => false,
        };
    }
}