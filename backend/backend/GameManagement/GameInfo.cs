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
}