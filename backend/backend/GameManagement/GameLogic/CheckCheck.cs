namespace backend.GameManagement.GameLogic;
using System.Numerics;
internal class CheckCheck
{
    //We use a struct to give along important iformations for further calculations
    /*
     The concept used in this function is to use the King as A super piece that can do all possible attack moves.
     if the King is able to capture an enemy piece that is able to do the used move, he is in check.
    
    (eg the king makes a diagonal move and is able to capture an enemy Queen or Bishop, he is in check since they'd
    be able to capture the King)
     */
    //Threat Board: Only add Pawns/Kings to the threat boards if they are really threatening --> Save special checks
    
    //This check must happen: on the old gameState, if we check the check bevor the move, and otherwise after all pieces moved.
    internal static bool PerformCheckCheck(GameInfo gameInfo, bool color)
    {
        //convert the Position of the King into an integer allowing us to use the following Arrays properly
        int kingPos = color?
            BitOperations.TrailingZeroCount(gameInfo.WKing) :
            BitOperations.TrailingZeroCount(gameInfo.BKing);
        
        //Checks if there exists a Knight wich threatens the King 
        ulong LThreat= color ? gameInfo.BKnight : gameInfo.WKnight;
        if ((LThreat & BitBoardPreCalculation.ThreadByKnightArray[kingPos]) != 0) return true;
        
        ulong threateningKingsAndPawns = color ? 
                (gameInfo.BPawn & BitBoardPreCalculation.ThreadByBlackPawnsArray[kingPos]) |
                (gameInfo.BKing & BitBoardPreCalculation.SurroundingBitBoardArray[kingPos]):
                
                (gameInfo.WPawn & BitBoardPreCalculation.ThreadByWhitePawnsArray[kingPos])|
                (gameInfo.WKing & BitBoardPreCalculation.SurroundingBitBoardArray[kingPos]);
        if(threateningKingsAndPawns!=0) return true;
        
        /* little reminder: Direction = 0 - 7, 0 = north, 1 = south, 2 = east, 3 = west, 4 = northeast,
        5 = southwest, 6 = southeast, 7= northwest, 8 = straight line, 9 = diagonal line*/
        
        //Caching the full Board
        ulong fullBoard=gameInfo.FullBitBoard;
        
        ulong straightThreat = color ?
            gameInfo.BQueen | gameInfo.BRook:
            gameInfo.WQueen | gameInfo.WRook;
        ulong[] threatArray= BitBoardPreCalculation.CheckThreatLineArray[kingPos];
        
        if((threatArray[8]&straightThreat) != 0){
            
            //North
            ulong directionalThreatNorth = threatArray[0];
            ulong piecesOnThreatLineNorth = directionalThreatNorth & fullBoard;
            ulong threatsOnThreatLineNorth = directionalThreatNorth & straightThreat;
            if (threatsOnThreatLineNorth != 0 &&
                BitOperations.TrailingZeroCount(piecesOnThreatLineNorth) ==
                BitOperations.TrailingZeroCount(threatsOnThreatLineNorth))
                return true;

            //South
            ulong directionalThreatSouth = threatArray[1];
            ulong piecesOnThreatLineSouth = directionalThreatSouth & fullBoard;
            ulong threatsOnThreatLineSouth = directionalThreatSouth & straightThreat;
            if (threatsOnThreatLineSouth != 0 &&
                BitOperations.LeadingZeroCount(piecesOnThreatLineSouth) ==
                BitOperations.LeadingZeroCount(threatsOnThreatLineSouth))
                return true;

            //East
            ulong directionalThreatEast = threatArray[2];
            ulong piecesOnThreatLineEast = directionalThreatEast & fullBoard;
            ulong threatsOnThreatLineEast = directionalThreatEast & straightThreat;
            if (threatsOnThreatLineEast != 0 &&
                BitOperations.TrailingZeroCount(piecesOnThreatLineEast) ==
                BitOperations.TrailingZeroCount(threatsOnThreatLineEast))
                return true;

            //West
            ulong directionalThreatWest = threatArray[3];
            ulong piecesOnThreatLineWest = directionalThreatWest & fullBoard;
            ulong threatsOnThreatLineWest = directionalThreatWest & straightThreat;
            if (threatsOnThreatLineWest != 0 &&
                BitOperations.LeadingZeroCount(piecesOnThreatLineWest) ==
                BitOperations.LeadingZeroCount(threatsOnThreatLineWest))
                return true;
        }


        ulong diagonalThreat = color ?
            gameInfo.BQueen | gameInfo.BBishop: 
            gameInfo.WQueen | gameInfo.WBishop;

        if ((threatArray[9] & diagonalThreat) != 0)
        {
            //Northeast
            ulong directionalThreatNortheast = threatArray[4];
            ulong piecesOnThreatLineNortheast = directionalThreatNortheast & fullBoard;
            ulong threatsOnThreatLineNortheast = directionalThreatNortheast & diagonalThreat;
            if (threatsOnThreatLineNortheast != 0 &&
                BitOperations.TrailingZeroCount(piecesOnThreatLineNortheast) ==
                BitOperations.TrailingZeroCount(threatsOnThreatLineNortheast))
                return true;

            //Southwest
            ulong directionalThreatSouthwest = threatArray[5];
            ulong piecesOnThreatLineSouthwest = directionalThreatSouthwest & fullBoard;
            ulong threatsOnThreatLineSouthwest = directionalThreatSouthwest & diagonalThreat;
            if (threatsOnThreatLineSouthwest != 0 &&
                BitOperations.LeadingZeroCount(piecesOnThreatLineSouthwest) ==
                BitOperations.LeadingZeroCount(threatsOnThreatLineSouthwest))
                return true;

            //Southeast
            ulong directionalThreatSoutheast = threatArray[6];
            ulong piecesOnThreatLineSoutheast = directionalThreatSoutheast & fullBoard;
            ulong threatsOnThreatLineSoutheast = directionalThreatSoutheast & diagonalThreat;
            if (threatsOnThreatLineSoutheast != 0 &&
                BitOperations.LeadingZeroCount(piecesOnThreatLineSoutheast) ==
                BitOperations.LeadingZeroCount(threatsOnThreatLineSoutheast))
                return true;

            //Northwest
            ulong directionalThreatNorthwest = threatArray[7];
            ulong piecesOnThreatLineNorthwest = directionalThreatNorthwest & fullBoard;
            ulong threatsOnThreatLineNorthwest = directionalThreatNorthwest & diagonalThreat;
            if (threatsOnThreatLineNorthwest != 0 &&
                BitOperations.TrailingZeroCount(piecesOnThreatLineNorthwest) ==
                BitOperations.TrailingZeroCount(threatsOnThreatLineNorthwest))
                return true;
        }

        //No Threats detected --> King is not in Check
        return false;
    }
}