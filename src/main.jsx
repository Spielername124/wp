import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import TTT  from './assets/ticTacToeFromTutorial.jsx'
import "./assets/styless.css";


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <TTT />
  </StrictMode>,
)