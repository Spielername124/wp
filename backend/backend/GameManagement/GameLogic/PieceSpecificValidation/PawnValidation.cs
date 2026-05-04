using System.Diagnostics;

namespace backend.GameManagement.GameLogic.PieceSpecificValidation;

internal static class PawnValidation
{
    //TODO rewrite it with pre-calculated Bitboards
    internal static bool ValidatePawn(GameInfo gameinfo, int originField, int targetField, bool color)
    {
        //vertical moves
        if (originField % 8 == targetField)
        {
            //Check if the targeted loaction is already occupied by an enemy, making the move invalid
            ulong enemyTable = GeneralBitBoardHelper.GetColorsBoard(gameinfo, !color);
            if((GeneralBitBoardHelper.BitBoardOnIndex(targetField)& enemyTable) !=0) return false;
            
            int offset = targetField - originField;
            return (color, offset) switch
            {
                (true, 8) => true,
                (true, 16) 
                    => ValidationHelper.IsValidVerticalMove(gameinfo, originField, targetField) && PawnHasNotMovedYet(gameinfo, originField, color),
                (false, -8) => true,
                (false, -16) 
                    => ValidationHelper.IsValidVerticalMove(gameinfo, originField, targetField) && PawnHasNotMovedYet(gameinfo, originField, color),
                _ => false
            };
        }
        //Diagonal moves
        else
        {
            //TODO add Bitboard to GameInfo which contains TargetIndexes that are en passantable. 
            ulong mockEnPassantableTable = 0;
            ulong enemyTable = mockEnPassantableTable | GeneralBitBoardHelper.GetColorsBoard(gameinfo, !color);
            
            int offset = targetField- originField;
            
            //rejects move if the pawn does not move in the horizontally right direction, or it doesn't move correctly diagonal
            if(!(
                (color && (offset == 7 || offset == 9)) ||
                (!color && (offset == -7 || offset == -9))))
                return false;
            //returns true if the moves captures a piece (--> makes the move valid) 
            return ((enemyTable & GeneralBitBoardHelper.BitBoardOnIndex(targetField)) != 0);
        }
    }

    internal static bool PawnHasNotMovedYet(GameInfo gameInfo, int originField, bool color)
    {
        return (color) switch
        {
            (true) => (GeneralBitBoardHelper.BitBoardOnIndex(originField)& gameInfo.WPawn) != 0,
            (false) => (GeneralBitBoardHelper.BitBoardOnIndex(originField) & gameInfo.BPawn) != 0
        };
    }
    
}