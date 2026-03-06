using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.GameManagement;


[Table("gameinfos")]
public class GameInfo
{
    [Key]
    [Column("gameid")]
    public int GameId { get; set; }
    [Column("playerid")]
    public int PlayerId { get; set; }
    [Column("opponentid")]
    public int OpponentId { get; set; }
    [Column("gamestate")]
    public string GameState { get; set;} ="default";

    [Column("turn")] 
    public int Turn { get; set; } = 0; // Shows how many turns have passed
    
    [Column("hasterminated")]
    public bool HasTerminated { get; set; } = false;
}