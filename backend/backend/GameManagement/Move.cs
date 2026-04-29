namespace backend.GameManagement;

public class Move
{
    public int MoveId{get;set;}
    public int GameId {get;set;}
    public bool MovingPlayer {get;set;} // True = white | False = black
    public int OriginField {get;set;} 
    public int TargetedField {get;set;}
    public char MovingPieceType{get;set;} //p=Pawn, r=Rook, h=Knight, b=Bishop, q=Queen, k=King
    
    //TODO: Add nullable promotion type for Pawn promotion
}