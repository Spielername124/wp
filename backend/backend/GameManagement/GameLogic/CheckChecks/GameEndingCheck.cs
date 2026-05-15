using System.Numerics;
using backend.GameManagement.GameLogic.CheckChecks.PseudoLegalMovesHelper;

namespace backend.GameManagement.GameLogic.CheckChecks;
/*TODO Handle gameEnding Scenarios like repeated moves, no mat makable able Pieces on both sides or repeated movements*/
public static class GameEndingCheck
{
    //returns code 0 if the game keeps active, 1 if a checkmate is detected, 2 if a remis is detected.
    // the Threats are calculated from the perspective of the enemy of this move --> enemy color is the moving color
    internal static int CheckGameEnding(GameInfo gameInfo, bool color)
    {
        var checkContext=
            CalculateCheckContext.PerformCheckContextCheck(gameInfo, color);
        ulong alliedBitboard = color ? gameInfo.WhiteBitBoard : gameInfo.BlackBitBoard;
        ulong threateningPieces = checkContext.ThreateningPieces;
        ulong pinnedPieces = checkContext.PinnedPieces;
        int kingPos = color
            ? BitOperations.TrailingZeroCount(gameInfo.WKing)
            : BitOperations.TrailingZeroCount(gameInfo.BKing);
        
        if (threateningPieces == 0)
        {
            //if there is no thread we must check if there is a remis
            return CalculateRemisByNoMoves(gameInfo, color, threateningPieces,
                pinnedPieces, alliedBitboard,kingPos)
                ? 2
                : 0;
        }
        else
        {
            // check if the enemy is able to postpone the checkmate by moving something.
            return CalculateCheckmate(gameInfo, color, threateningPieces,
                pinnedPieces, alliedBitboard, kingPos)
                ? 1
                : 0;
        }
    }

   private static bool CalculateCheckmate(GameInfo gameInfo, bool color, ulong threateningPieces,
        ulong pinnedPieces, ulong alliedBitboard, int kingPos)
    {
        
        //if there is more than one thread, no blocking will be enough to safe the king. He has to move.
        if (BitOperations.PopCount(threateningPieces) > 1)
            return !KingPseudoLegalMoves.CanKingMove(gameInfo, color, alliedBitboard, kingPos);
        
        //we calculate the bitboard showing where a piece could move to, to block the existing thread.
        ulong blockingLine = threateningPieces|
                             BitBoardPreCalculation.LOSBitboardArray[kingPos, BitOperations.TrailingZeroCount(threateningPieces)];
        
        
        return (blockingLine&
               GeneralPseudoLegalMoves.GetPseudoLegalMoves(gameInfo, color, threateningPieces, pinnedPieces, alliedBitboard)) !=0 ||
               KingPseudoLegalMoves.CanKingMove(gameInfo, color,alliedBitboard, kingPos);
    }

    private static bool CalculateRemisByNoMoves(GameInfo gameInfo, bool color, ulong threateningPieces,
        ulong pinnedPieces, ulong alliedBitboard,int kingPos)
    {
        return (GeneralPseudoLegalMoves.GetPseudoLegalMoves(gameInfo, color, threateningPieces, pinnedPieces, alliedBitboard)) !=0 ||
               KingPseudoLegalMoves.CanKingMove(gameInfo, color,alliedBitboard, kingPos);
    }
}