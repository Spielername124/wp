using System.Numerics;

namespace backend.GameManagement.GameLogic;

public static class CalculateCheckContext
{
    
    //We use a struct to give along important information for further calculations
    public readonly struct ContextCheck(ulong pinnedPieces, ulong threateningPieces)

    {
        //IMPORTANT NOTE: The PinnedPieces table contains pinned friends AND foes. This is intended.
        
        public readonly ulong PinnedPieces = pinnedPieces;

        public readonly ulong ThreateningPieces = threateningPieces;

    } 
    
    internal static ContextCheck PerformCheckContextCheck(GameInfo gameInfo, bool color)
    {
        ulong pinnedPieces = 0;
        ulong threateningPieces = 0;
        
        
        //convert the Position of the King into an integer allowing us to use the following Arrays properly
        int kingPos = color?
            BitOperations.TrailingZeroCount(gameInfo.WKing) :
            BitOperations.TrailingZeroCount(gameInfo.BKing);
        
        //Checks if there exists a Knight wich threatens the King 
        ulong LThreat= color ? gameInfo.BKnight : gameInfo.WKnight;
        threateningPieces = LThreat;
        
        ulong threateningKingsAndPawns = color ? 
                (gameInfo.BPawn & BitBoardPreCalculation.ThreadByBlackPawnsArray[kingPos]) |
                (gameInfo.BKing & BitBoardPreCalculation.SurroundingBitBoardArray[kingPos]):
                
                (gameInfo.WPawn & BitBoardPreCalculation.ThreadByWhitePawnsArray[kingPos])|
                (gameInfo.WKing & BitBoardPreCalculation.SurroundingBitBoardArray[kingPos]);
        threateningPieces |= threateningKingsAndPawns;
        
        /* little reminder: Direction = 0 - 7, 0 = north, 1 = south, 2 = east, 3 = west, 4 = northeast,
        5 = southwest, 6 = southeast, 7= northwest, 8 = straight line, 9 = diagonal line*/
        
        //Caching the full Board
        ulong fullBoard=gameInfo.FullBitBoard;
        
        ulong straightThreat = color ?
            gameInfo.BQueen | gameInfo.BRook:
            gameInfo.WQueen | gameInfo.WRook;
        ulong[] threatArray= BitBoardPreCalculation.CheckThreatLineArray[kingPos];
        
        //caching the LOS Board
        ulong[,] lOSboard = BitBoardPreCalculation.LOSBitboardArray;
        
        if((threatArray[8]&straightThreat) != 0){
            
            //North
            ulong directionalThreatNorth = threatArray[0];
            if ((directionalThreatNorth & straightThreat) != 0)
            {
                ulong threatsOnThreatLineNorth = directionalThreatNorth & straightThreat;
                int firstEnemyContact = BitOperations.TrailingZeroCount(threatsOnThreatLineNorth);
                ulong blockerOnThreatLineNorth = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineNorth))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineNorth;
                        break;
                }
            }

            //South
            ulong directionalThreatSouth = threatArray[1];
            if ((directionalThreatSouth & straightThreat) != 0)
            {
                ulong threatsOnThreatLineSouth = directionalThreatSouth & straightThreat;
                int firstEnemyContact = 63 - BitOperations.LeadingZeroCount(threatsOnThreatLineSouth);
                ulong blockerOnThreatLineSouth = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineSouth))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineSouth;
                        break;
                }
            }

            //East
            ulong directionalThreatEast = threatArray[2];
            if ((directionalThreatEast & straightThreat) != 0)
            {
                ulong threatsOnThreatLineEast = directionalThreatEast & straightThreat;
                int firstEnemyContact = BitOperations.TrailingZeroCount(threatsOnThreatLineEast);
                ulong blockerOnThreatLineEast = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineEast))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineEast;
                        break;
                }
            }

            //West
            ulong directionalThreatWest = threatArray[3];
            if ((directionalThreatWest & straightThreat) != 0)
            {
                ulong threatsOnThreatLineWest = directionalThreatWest & straightThreat;
                int firstEnemyContact = 63 - BitOperations.LeadingZeroCount(threatsOnThreatLineWest);
                ulong blockerOnThreatLineWest = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineWest))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineWest;
                        break;
                }
            }
        }


        ulong diagonalThreat = color ?
            gameInfo.BQueen | gameInfo.BBishop: 
            gameInfo.WQueen | gameInfo.WBishop;

        if ((threatArray[9] & diagonalThreat) != 0)
        {
            //Northeast
            ulong directionalThreatNortheast = threatArray[4];
            if ((directionalThreatNortheast & diagonalThreat) != 0)
            {
                ulong threatsOnThreatLineNortheast = directionalThreatNortheast & diagonalThreat;
                int firstEnemyContact = BitOperations.TrailingZeroCount(threatsOnThreatLineNortheast);
                ulong blockerOnThreatLineNortheast = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineNortheast))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineNortheast;
                        break;
                }
            }

            //Southwest
            ulong directionalThreatSouthwest = threatArray[5];
            if ((directionalThreatSouthwest & diagonalThreat) != 0)
            {
                ulong threatsOnThreatLineSouthwest = directionalThreatSouthwest & diagonalThreat;
                int firstEnemyContact = 63 - BitOperations.LeadingZeroCount(threatsOnThreatLineSouthwest);
                ulong blockerOnThreatLineSouthwest = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineSouthwest))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineSouthwest;
                        break;
                }
            }

            //Southeast
            ulong directionalThreatSoutheast = threatArray[6];
            if ((directionalThreatSoutheast & diagonalThreat) != 0)
            {
                ulong threatsOnThreatLineSoutheast = directionalThreatSoutheast & diagonalThreat;
                int firstEnemyContact = 63 - BitOperations.LeadingZeroCount(threatsOnThreatLineSoutheast);
                ulong blockerOnThreatLineSoutheast = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineSoutheast))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineSoutheast;
                        break;
                }
            }

            //Northwest
            ulong directionalThreatNorthwest = threatArray[7];
            if ((directionalThreatNorthwest & diagonalThreat) != 0)
            {
                ulong threatsOnThreatLineNorthwest = directionalThreatNorthwest & diagonalThreat;
                int firstEnemyContact = BitOperations.TrailingZeroCount(threatsOnThreatLineNorthwest);
                ulong blockerOnThreatLineNorthwest = lOSboard[kingPos, firstEnemyContact] & fullBoard;
                switch (BitOperations.PopCount(blockerOnThreatLineNorthwest))
                {
                    case 0:
                        threateningPieces |= 1UL << firstEnemyContact;
                        break;
                    case 1:
                        pinnedPieces |= blockerOnThreatLineNorthwest;
                        break;
                }
            }
        }


        return new ContextCheck(pinnedPieces, threateningPieces);
    }
    
}