using System.Reflection.Metadata.Ecma335;

namespace backend.GameManagement.GameLogic;

public static class MoveExecution
{
    public static void ExecuteMove(GameInfo gameInfo, Move move)
    {
        if (IsPawnPromotion(gameInfo, move))
        {
            //TODO
            return;
        }

        if (IsRochade(gameInfo, move))
        {
            //TODO
            return;
        }
        // deletes the piece at the prior location and creats a new one at the new location
        MovePiece(gameInfo, move.MovingPieceType,move.MovingPlayer,move.OriginField,move.TargetedField);
        
        //If there exists an enemy on the Square, it gets deleted, else nothing happens
        DeletePiece(gameInfo, !move.MovingPlayer, move.TargetedField);
    }
    
    private static bool IsRochade(GameInfo gameInfo, Move move)
    {
        //TODO
        return false;
    }
    
    private static bool IsPawnPromotion(GameInfo gameInfo, Move move)
    {
        //TODO
        return false;
    }

    private static void DeletePiece(GameInfo gameInfo, bool color, int index)
    {
        ulong toRemoveBitboard = BitBoardHelper.BitBoardOnIndex(index);

        if (color)
        {
            gameInfo.WPawn &= ~toRemoveBitboard;
            gameInfo.WRook &= ~toRemoveBitboard;
            gameInfo.WKnight &= ~toRemoveBitboard;
            gameInfo.WBishop &= ~toRemoveBitboard;
            gameInfo.WQueen &= ~toRemoveBitboard;
            gameInfo.WKing &= ~toRemoveBitboard;
        }
        else
        {
            gameInfo.BPawn &= ~toRemoveBitboard;
            gameInfo.BRook &= ~toRemoveBitboard;
            gameInfo.BKnight &= ~toRemoveBitboard;
            gameInfo.BBishop &= ~toRemoveBitboard;
            gameInfo.BQueen &= ~toRemoveBitboard;
            gameInfo.BKing &= ~toRemoveBitboard;
        }
    }

    private static void MovePiece(GameInfo gameInfo, char type, bool color, int originField, int targetedField)
    {
        ulong moveMask= BitBoardHelper.BitBoardOnIndex(originField) | BitBoardHelper.BitBoardOnIndex(targetedField);
        _ = (color, type) switch
        {
            (true, 'p') => gameInfo.WPawn ^= moveMask,
            (true, 'r') => gameInfo.WRook ^= moveMask,
            (true, 'h') => gameInfo.WKnight ^= moveMask,
            (true, 'b') => gameInfo.WBishop ^= moveMask,
            (true, 'q') => gameInfo.WQueen ^= moveMask,
            (true, 'k') => gameInfo.WKing ^= moveMask,
            (false, 'p') => gameInfo.BPawn ^= moveMask,
            (false, 'r') => gameInfo.BRook ^= moveMask,
            (false, 'h') => gameInfo.BKnight ^= moveMask,
            (false, 'b') => gameInfo.BBishop ^= moveMask,
            (false, 'q') => gameInfo.BQueen ^= moveMask,
            (false, 'k') => gameInfo.BKing ^= moveMask,
            _ => throw new ArgumentException()
        };
    }
    
}