namespace backend.GameInfo;

public class GameInfo
{
    public int GameId { get; set; }
    public int? Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public int TurnCounter { get; set; }
    public bool HasTerminated { get; set; }
    
    public long WPawn { get; set; }
    public long WKnight { get; set; }
    public long WBishop { get; set; }
    public long WRook { get; set; }
    public long WQueen { get; set; }
    public long WKing { get; set; }

    public long BPawn { get; set; }
    public long BKnight { get; set; }
    public long BBishop { get; set; }
    public long BRook { get; set; }
    public long BQueen { get; set; }
    public long BKing { get; set; }
}