import { useState, useEffect } from 'react';
import {useNavigate} from "react-router-dom";

export default function GameSelector() {
    const navigate = useNavigate();

    const handleJoinGame = () => {
        // Beispiel-Daten, die Sie mitnehmen möchten
        const GameInformation = {
            gameID: 1,
            playerID: 2,
            OpponentID: 1,
            createNewGame: false,

        };

        // Navigation mit State-Übergabe
        navigate('/chess', { state: GameInformation });
    };
    const handleCreateChessGame = () => {
        // Beispiel-Daten, die Sie mitnehmen möchten
        //TODO: Create new game in Backend, then join
        const GameInformation = {
            gameID: 1, //useless
            playerID: 1,
            OpponentID: 2,
            createNewGame: true,
        };

        // Navigation mit State-Übergabe
        navigate('/chess', { state: GameInformation });
    };

    return (
        <div>
            <button onClick={handleCreateChessGame}>Instanz erstllen</button>
            <button onClick={handleJoinGame}>Instanz beitreten</button>
        </div>
    );

}

