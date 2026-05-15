namespace backend.GameManagement.GameLogic.CheckChecks.PseudoLegalMovesHelper;

public static class GeneralPseudoLegalMoves
{
    //Make sure that Pinned Pieces are not able to move
    public static ulong GetPseudoLegalMoves(GameInfo gameInfo, bool color, ulong threateningPieces,
        ulong pinnedPieces, ulong alliedBitboard)
    {
        ulong fullBitBoard = gameInfo.FullBitBoard;
        ulong capturableByWhitePawns = gameInfo.BlackBitBoard& gameInfo.EnPassantVulnerable;
        ulong capturableByBlackPawns = gameInfo.WhiteBitBoard& gameInfo.EnPassantVulnerable;
        //by definition, pawns may not be in the last line of the field --> no rollover mask for 1 step forward.
        ulong pseudoLegalMoves = color
            ?
            //we take every non pinned pawn and shift it a one step up, but don't move if the target fiel is already occupied
            (((gameInfo.WPawn & ~pinnedPieces) << 8) & ~fullBitBoard)|
            /*we get the pawns that are valid to attack to the eastern side
            This Hex ulong is a mask where north, east and south are 0 the rest 1. 
            (this are valid positions for pawns that might perform an east capture)*/
            (((gameInfo.WPawn & 0x007F7F7F7F7F7F00UL)
            //we shift it to northeast to perform a hypothetical capture
            << 9)
                //and then check if there actually is an enemy to capture 
                & capturableByWhitePawns) |
            
            //we do the same for the west directed captures
            (((gameInfo.WPawn & 0x00FEFEFEFEFEFE00UL) << 7) & capturableByWhitePawns)|
            
            //gets the pawns that have not moved yet and are not pinned, and shift them to the possible location
            ((((gameInfo.WPawn&0x000000000000FF00UL)& (~pinnedPieces))<<16)&
            //Get the positions where there are neither pieces in the third nor fourth row
            (~fullBitBoard & ~(fullBitBoard<<8)))
            
            :
            (((gameInfo.BPawn & ~pinnedPieces)>>8)& ~fullBitBoard)|
            
            (((gameInfo.BPawn & 0x00FEFEFEFEFEFE00UL) >> 9) & capturableByBlackPawns)|
            (((gameInfo.BPawn & 0x007F7F7F7F7F7F00UL) >> 7) & capturableByBlackPawns)|
            
            ((((gameInfo.BPawn&0x00FF000000000000UL)& (~pinnedPieces))>>16)&
             (~fullBitBoard & ~(fullBitBoard>>8)));
        
        

        
        
        
        
        /*TODO: find a solution how to handle possibly legal moves, done by pinned pieces.
         Possible solution: Use a different bitboard for each axis and allow the piece to move along
         his blocking axis*/
        
        return 0;
    }
    
}