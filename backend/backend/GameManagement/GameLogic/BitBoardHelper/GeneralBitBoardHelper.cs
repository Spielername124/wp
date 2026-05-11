namespace backend.GameManagement.GameLogic;

public static class GeneralBitBoardHelper
{
    
    public static ulong BitBoardOnIndex(int index)
    {
        if (index < 0 || index >= 64) throw new InvalidDataException();
        return 1UL << index;
    }

    public static ulong GetPieceBitboard(GameInfo gameInfo, char type, bool color)
    {
        return (color, type) switch
        {
            (true, 'p') => gameInfo.WPawn,
            (true, 'r') => gameInfo.WRook,
            (true, 'h') => gameInfo.WKnight,
            (true, 'b') => gameInfo.WBishop,
            (true, 'q') => gameInfo.WQueen,
            (true, 'k') => gameInfo.WKing,
            (false, 'p') => gameInfo.BPawn,
            (false, 'r') => gameInfo.BRook,
            (false, 'h') => gameInfo.BKnight,
            (false, 'b') => gameInfo.BBishop,
            (false, 'q') => gameInfo.BQueen,
            (false, 'k') => gameInfo.BKing,
            _ => throw new ArgumentException()
        };
    }

    public static ulong GetColorsBoard(GameInfo gameInfo, bool color)
    {
        return color
            ? (
                gameInfo.WhiteBitBoard
            ) : (
                gameInfo.BlackBitBoard
               );

    }
    
}

    