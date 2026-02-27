import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import TTT  from './legacyJunk/ticTacToeFromTutorial.jsx'
import "./style/styles.css";
import Chess from "./chessIngame/assets/chess.jsx";
import GameSelector from "./chessPregame/GameSelector.jsx";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <Router>
            <Routes>
                <Route path="/" element={<GameSelector />} />
                <Route path="/chess" element={<Chess />} />
            </Routes>
        </Router>
    </StrictMode>
);