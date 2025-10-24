import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import TTT  from './assets/ticTacToeFromTutorial.jsx'
import "./assets/styless.css";
import Chess from "./assets/chess.jsx";



createRoot(document.getElementById('root')).render(
  <StrictMode>
     <Chess />
  </StrictMode>,
)