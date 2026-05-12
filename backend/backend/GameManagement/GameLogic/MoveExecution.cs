using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace backend.GameManagement.GameLogic;
//IMPORTANT: Always remove pieces on capture before moving the new piece.
public static class MoveExecution
{
    public static void ExecuteMove(GameInfo gameInfo, Move move)
    {
        if (IsPawnPromotion(move.MovingPlayer,move.TargetedField))
        {
            if (move.PromotionType == null) throw new InvalidEnumArgumentException();
            
            DeletePiece(gameInfo, !move.MovingPlayer, move.TargetedField);
            DeletePiece(gameInfo, !move.MovingPlayer, move.OriginField);
            SpawnPiece(gameInfo, move.PromotionType.Value, move.MovingPlayer,move.TargetedField);
            return;
        }
        
        if (IsCasteling(gameInfo, move))
        {
            switch (move.MovingPlayer, move.TargetedField)
            {
                case(true,2):
                    MovePiece(gameInfo, 'k', true, move.OriginField, move.TargetedField);
                    MovePiece(gameInfo, 'r', true, 0, move.TargetedField+1);
                    break;
                case(true,6): 
                    MovePiece(gameInfo, 'k', true, move.OriginField, move.TargetedField);
                    MovePiece(gameInfo, 'r', true, 7, move.TargetedField-1);
                    break;
                
                case(false,58): 
                    MovePiece(gameInfo, 'k', false, move.OriginField, move.TargetedField);
                    MovePiece(gameInfo, 'r', false, 56, move.TargetedField+1);
                    break;
                case(false,62): 
                    MovePiece(gameInfo, 'k', false, move.OriginField, move.TargetedField);
                    MovePiece(gameInfo, 'r', false, 63, move.TargetedField-1);
                    break;
            }
            return;
        }

        if (IsEnPassant(gameInfo, move))
        {
            //delete the en passantable piece
            if(move.MovingPlayer) gameInfo.BlackBitBoard ^= GeneralBitBoardHelper.BitBoardOnIndex(move.TargetedField+8);
            else gameInfo.WhiteBitBoard ^= GeneralBitBoardHelper.BitBoardOnIndex(move.TargetedField-8);
        };
        
        //If there exists an enemy on the Square, it gets deleted, else nothing happens
        DeletePiece(gameInfo, !move.MovingPlayer, move.TargetedField);
        // deletes the piece at the prior location and create a new one at the new location
        MovePiece(gameInfo, move.MovingPieceType,move.MovingPlayer,move.OriginField,move.TargetedField);
        
        //generate the enpassantable board
        if (move.MovingPieceType == 'b' && Math.Abs(move.TargetedField - move.OriginField) == 16)
        {
            gameInfo.EnPassantVulnerable =
                GeneralBitBoardHelper.BitBoardOnIndex(Math.Min(move.TargetedField, move.OriginField) + 8);
        }
        else gameInfo.EnPassantVulnerable = 0;
        
        //romove form hasNotMoved if it has moved
        if((gameInfo.HasNotMoved & GeneralBitBoardHelper.BitBoardOnIndex(move.OriginField))!=0)
            gameInfo.HasNotMoved ^= GeneralBitBoardHelper.BitBoardOnIndex(move.OriginField);
    }
    
    private static bool IsCasteling(GameInfo gameInfo, Move move)
    {
        return (Math.Abs(move.OriginField - move.TargetedField)>1);
    }
    
    private static bool IsPawnPromotion( bool color, int index)
    {
        return (color) switch
        {
            // tests if the (potential) white pawn is now on the other siede of the board
            (true) => (GeneralBitBoardHelper.BitBoardOnIndex(index)& (ulong) 72057594037927936) != 0,
            
            // tests if the (potential) black pawn is on the other side of the board
            (false) => (GeneralBitBoardHelper.BitBoardOnIndex(index)& (ulong) 128) != 0,
        };
    }

    private static bool IsEnPassant(GameInfo gameInfo, Move move)
        //TODO There has to be an better way to do this. Pls do at some point
    {
        return (
            (GeneralBitBoardHelper.BitBoardOnIndex(move.TargetedField) & gameInfo.EnPassantVulnerable) == 0)&&
               move.MovingPieceType=='b' && (GeneralBitBoardHelper.BitBoardOnIndex(move.OriginField)&gameInfo.HasNotMoved)==0;
    }

    private static void DeletePiece(GameInfo gameInfo, bool color, int index)
    {
        ulong toRemoveBitboard = ~(GeneralBitBoardHelper.BitBoardOnIndex(index));
        if (color)
        {
            //remove the piece in all specific Boards
            gameInfo.WPawn &= toRemoveBitboard;
            gameInfo.WRook &= toRemoveBitboard;
            gameInfo.WKnight &= toRemoveBitboard;
            gameInfo.WBishop &= toRemoveBitboard;
            gameInfo.WQueen &= toRemoveBitboard;
            gameInfo.WKing &= toRemoveBitboard;
            //remove the piece on the redundant Boards
            gameInfo.WhiteBitBoard &= toRemoveBitboard;
            gameInfo.FullBitBoard &= toRemoveBitboard;
        }
        else
        {
            //remove the piece in all specific Boards
            gameInfo.BPawn &= toRemoveBitboard;
            gameInfo.BRook &= toRemoveBitboard;
            gameInfo.BKnight &= toRemoveBitboard;
            gameInfo.BBishop &= toRemoveBitboard;
            gameInfo.BQueen &= toRemoveBitboard;
            gameInfo.BKing &= toRemoveBitboard;
            //remove the piece on the redundant Boards
            gameInfo.BlackBitBoard &= toRemoveBitboard;
            gameInfo.FullBitBoard &= toRemoveBitboard;
        }
    }

    private static void MovePiece(GameInfo gameInfo, char type, bool color, int originField, int targetedField)
    {
        ulong moveMask= GeneralBitBoardHelper.BitBoardOnIndex(originField) | GeneralBitBoardHelper.BitBoardOnIndex(targetedField);
        _ = (color, type) switch
        {
            (true, 'r') => gameInfo.WRook ^= moveMask,
            (true, 'h') => gameInfo.WKnight ^= moveMask,
            (true, 'b') => gameInfo.WBishop ^= moveMask,
            (true, 'q') => gameInfo.WQueen ^= moveMask,
            
            (false, 'r') => gameInfo.BRook ^= moveMask,
            (false, 'h') => gameInfo.BKnight ^= moveMask,
            (false, 'b') => gameInfo.BBishop ^= moveMask,
            (false, 'q') => gameInfo.BQueen ^= moveMask,
            _ => throw new ArgumentException()
        };
        if (color) gameInfo.WhiteBitBoard ^= moveMask;
        else gameInfo.BlackBitBoard ^= moveMask;
        gameInfo.FullBitBoard ^= moveMask;
    }

    internal static void SpawnPiece(GameInfo gameInfo, char type, bool color, int index)
    {
        ulong spawnMask = GeneralBitBoardHelper.BitBoardOnIndex(index);
        _ = (color, type) switch
        {
            (true, 'p') => gameInfo.WPawn |= spawnMask,
            (true, 'r') => gameInfo.WRook |= spawnMask,
            (true, 'h') => gameInfo.WKnight |= spawnMask,
            (true, 'b') => gameInfo.WBishop |= spawnMask,
            (true, 'q') => gameInfo.WQueen |= spawnMask,
            (true, 'k') => gameInfo.WKing |= spawnMask,
            (false, 'p') => gameInfo.BPawn |= spawnMask,
            (false, 'r') => gameInfo.BRook |= spawnMask,
            (false, 'h') => gameInfo.BKnight |= spawnMask,
            (false, 'b') => gameInfo.BBishop |= spawnMask,
            (false, 'q') => gameInfo.BQueen |= spawnMask,
            (false, 'k') => gameInfo.BKing |= spawnMask,
            _ => throw new ArgumentException()

        };
        
        //Add the piece to the overview Boards
        if (color) gameInfo.WhiteBitBoard |= spawnMask;
        else gameInfo.BlackBitBoard |= spawnMask;
        gameInfo.FullBitBoard |= spawnMask;
    }
    
}