import { useState, useEffect } from 'react';
import {useNavigate} from "react-router-dom";

export default function GameSelector() {
    const navigate = useNavigate();

    const handleJoinGame = async () => {
        const id=1;
        try {
            const response = await fetch(`/backend/gameinfo/${id}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (response.status === 404) {
                console.error("Game not found.");
                return;
            }

            if (!response.ok) {
                throw new Error(`Server-Error: ${response.status}`);
            }

            const gameData = await response.json();

            navigate('/chess', { state: gameData });

        } catch (error) {
            console.error("Something went wrong :(", error);
        }

    };
    const handleCreateChessGame = async () => {
        const newGame = {
<<<<<<< HEAD
            playerId: 1,
            opponentId: 2,
=======
            Player1Id: 1,
            Player2Id: 2,
>>>>>>> 463f711bf444ae5d3bddb97621b049f9f2973bc0
        };

        try {
            const response = await fetch('/backend/gameinfo/', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newGame)
            });

            if (!response.ok) {
                throw new Error(`Server-Error: ${response.status}`);
            }

            const gameData = await response.json();

            navigate('/chess', { state: gameData });

        } catch (error) {
            console.error("Something went wrong :(", error);
        }
    };

    return (
        <div>
            <button onClick={handleCreateChessGame}>Instanz erstllen</button>
            <button onClick={handleJoinGame}>Instanz beitreten</button>
        </div>
    );

}

