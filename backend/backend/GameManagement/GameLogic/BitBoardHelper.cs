namespace backend.GameManagement.GameLogic;

public static class BitBoardHelper
{
    
    public static ulong BitBoardOnIndex(int index)
    {
        if (index < 0 || index >= 64) throw new InvalidDataException();
        return 1UL << index;
    }

    public static ulong GetPieceBitboard(GameInfo gameInfo, char type, bool color)
    {
        if (color)
        {
            if (type == 'p') return gameInfo.WPawn;
            if (type == 'r') return gameInfo.WRook;
            if (type == 'h') return gameInfo.WKnight;
            if (type == 'b') return gameInfo.WBishop;
            if (type == 'q') return gameInfo.WQueen;
            if (type == 'k') return gameInfo.WKing;
        }
        else
        {
            if (type == 'p') return gameInfo.BPawn;
            if (type == 'r') return gameInfo.BRook;
            if (type == 'h') return gameInfo.BKnight;
            if (type == 'b') return gameInfo.BBishop;
            if (type == 'q') return gameInfo.BQueen;
            if (type == 'k') return gameInfo.BKing;
        }
        throw new InvalidDataException();
    }

    public static ulong GetColorsBoard(GameInfo gameInfo, bool color)
    {
        return color
            ? (
                gameInfo.WPawn |
                gameInfo.WRook |
                gameInfo.WKing |
                gameInfo.WBishop |
                gameInfo.WQueen |
                gameInfo.WKing
            ) : (
                gameInfo.BPawn |
                gameInfo.BRook |
                gameInfo.BKing |
                gameInfo.BBishop |
                gameInfo.BQueen |
                gameInfo.BKing
               );

    }
}