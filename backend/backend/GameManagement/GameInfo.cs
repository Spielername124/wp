namespace backend.GameManagement;

//TODO: If bored, rename GameInfo to GameState
public class GameInfo
{
    public int GameId { get; set; }
    public int? Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public int TurnCounter { get; set; }
    public bool HasTerminated { get; set; }
    
    public ulong WPawn { get; set; }
    public ulong WKnight { get; set; }
    public ulong WBishop { get; set; }
    public ulong WRook { get; set; }
    public ulong WQueen { get; set; }
    public ulong WKing { get; set; }

    public ulong BPawn { get; set; }
    public ulong BKnight { get; set; }
    public ulong BBishop { get; set; }
    public ulong BRook { get; set; }
    public ulong BQueen { get; set; }
    public ulong BKing { get; set; }
}