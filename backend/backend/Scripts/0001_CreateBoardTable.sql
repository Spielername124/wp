CREATE TABLE game_info(
    game_id SERIAL PRIMARY KEY,
    player_1_id INT,
    player_2_id INT,
    turn_counter INT,
    has_terminated BOOLEAN,
    
    
    w_pawn BIGINT,
    w_knight BIGINT,
    w_bishop BIGINT,
    w_rook BIGINT,
    w_queen BIGINT,
    w_king BIGINT,

    b_pawn BIGINT,
    b_knight BIGINT,
    b_bishop BIGINT,
    b_rook BIGINT,
    b_queen BIGINT,
    b_king BIGINT
)