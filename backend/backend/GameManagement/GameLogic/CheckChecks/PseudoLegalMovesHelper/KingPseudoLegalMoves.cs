using System.Numerics;

namespace backend.GameManagement.GameLogic.CheckChecks.PseudoLegalMovesHelper;

public static class KingPseudoLegalMoves
{
    /*we generally don't want to use this method if there is any way of not using it. 
    Theoretically there seems to be an approach by a magic bitboard calculated AttackBitboard, but using 
    magic bitboards is currently not planed*/
    public static bool CanKingMove(GameInfo gameInfo, bool color, ulong alliedBitboard, int kingPos)
    {
        ulong possibleMoves = BitBoardPreCalculation.SurroundingBitBoardArray[kingPos];
        possibleMoves &= ~alliedBitboard;
        while (possibleMoves != 0)
        {
            int nextPossibleMove = BitOperations.TrailingZeroCount(possibleMoves);
            //if a move does not end in a check, there exists a valid move --> no mate.
            if (CheckCheck.PerformCheckCheck(gameInfo, color, nextPossibleMove))
                return true;
            possibleMoves <<= (nextPossibleMove + 1);
        }
        return false;
    }
    
}