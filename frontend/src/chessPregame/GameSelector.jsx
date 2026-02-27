import { useState, useEffect } from 'react';
import {useNavigate} from "react-router-dom";

export default function GameSelector() {
    const navigate = useNavigate();

    const handleStartChess = () => {
        // Beispiel-Daten, die Sie mitnehmen möchten
        const spielerInfo = {
            name: "Spieler1",
            level: 5,
            letzteTodo: "Schach lernen"
        };

        // Navigation mit State-Übergabe
        navigate('/chess', { state: spielerInfo });
    };

    return (
        <div>
            <h2>Todo Liste</h2>
            <button onClick={handleStartChess}>Zu Schach wechseln</button>
        </div>
    );

}

