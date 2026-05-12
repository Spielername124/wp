namespace backend.GameManagement;

//TODO: If bored, rename GameInfo to GameState
public class GameInfo
{
    //TODO add Bitboard to GameInfo which contains TargetIndexes that are en passantable
    //TODO add Bitboard of rooks and pawns that have not moved jet.
    
    /* TODO add BlackBoard / WhiteBoard bitboards and use them in the backend
            add fullOcoupancy board and use it in the backend
     */
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
    public ulong FullBitBoard { get; set; }
    public ulong BlackBitBoard { get; set; }
    public ulong WhiteBitBoard { get; set; }
    public ulong HasNotMoved { get; set; }
    //Contains 1 on the position where a En passant can happen. NOT where the en passantable enemy is.
    public ulong EnPassantVulnerable {set;get;}

    public GameInfo Clone()
    {
        return (GameInfo) this.MemberwiseClone();
    }
    
}