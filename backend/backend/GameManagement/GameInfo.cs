using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.GameManagement;


[Table("gameinfos")]
public class GameInfo
{
    [Key]
    [Column("gameId")]
    public int GameId { get; set; }
    [Column("playerId")]
    public int PlayerId { get; set; }
    [Column("opponentId")]
    public int OpponentId { get; set; }
    [Column("gameState")]
    public string GameState { get; set;} ="default"; 
}