using System.Data;

namespace backend.GameManagement.GameLogic;

public static class MoveValidation
{
    public static bool ValidateMove(Move move, GameInfo gameInfo)
    {
        //rejects the move if the moving piece doesn't exist on the requested index
        if((BitBoardHelper.BitBoardOnIndex(move.OriginField) &
            BitBoardHelper.GetPieceBitboard(gameInfo,move.MovingPieceType,move.MovingPlayer))==0)
            return false;
        
        //rejects move if an allied piece occupies the target square
        if (
            (BitBoardHelper.BitBoardOnIndex(move.TargetedField) &
             BitBoardHelper.GetColorsBoard(gameInfo, move.MovingPlayer)
            ) != 0
        ) return false;
        // TODO: reject move if moving player is check after moving
        
        
        return true;
    }
    
}