namespace backend.GameManagement;

public class Move
{
    public int MoveId{get;set;}
    public int GameId {get;set;}
    public bool MovingPlayer {get;set;} // True = white | False = black
    public int OriginField {get;set;}
    public int TargetedField {get;set;}
    public char MovingPieceType{get;set;}
}