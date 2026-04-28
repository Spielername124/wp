CREATE TABLE moves(
    
    move_id SERIAL PRIMARY KEY,
    game_id INT NOT NULL,
    moving_player BOOLEAN NOT NULL,
    origin_field INT NOT NULL,
    targeted_field INT NOT NULL,
    moving_piece_type CHAR NOT NULL
);
