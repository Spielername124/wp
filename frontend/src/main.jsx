import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import TTT  from './legacyJunk/ticTacToeFromTutorial.jsx'
import "./style/styles.css";
import Chess from "./assets/chess.jsx";



createRoot(document.getElementById('root')).render(
  <StrictMode>
     <Chess />
  </StrictMode>,
)