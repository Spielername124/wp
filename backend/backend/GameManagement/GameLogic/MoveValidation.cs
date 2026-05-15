using System.Data;
using System.Numerics;
using backend.GameManagement.GameLogic.CheckChecks;

namespace backend.GameManagement.GameLogic;

public static class MoveValidation
{
    public static bool ValidateMove(Move move, GameInfo gameInfo)
    {
        //rejects the move if the moving piece doesn't exist on the requested index
        if((GeneralBitBoardHelper.BitBoardOnIndex(move.OriginField) &
            GeneralBitBoardHelper.GetPieceBitboard(gameInfo,move.MovingPieceType,move.MovingPlayer))==0)
            return false;
        
        //rejects move if an allied piece occupies the target square
        if (
            (GeneralBitBoardHelper.BitBoardOnIndex(move.TargetedField) &
             GeneralBitBoardHelper.GetColorsBoard(gameInfo, move.MovingPlayer)
            ) != 0
        ) return false;
        //Safe pre-move game state for the case that 
        GameInfo preMove = gameInfo.Clone();
        //Execute the Move
        MoveExecution.ExecuteMove(gameInfo, move);
        //undo the execution and reject the move
        int kingPos = move.MovingPlayer?
            BitOperations.TrailingZeroCount(gameInfo.WKing) :
            BitOperations.TrailingZeroCount(gameInfo.BKing);
        if (CheckCheck.PerformCheckCheck(gameInfo, move.MovingPlayer, kingPos))
        {
            gameInfo=preMove;
            return false;
        }
        return true;
    }
    
}