import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import TTT  from './legacyJunk/ticTacToeFromTutorial.jsx'
import "./style/styles.css";
import Chess from "./assets/chess.jsx";
import TodoApp from "./legacyJunk/toDoTutorialBackendCom.jsx";

createRoot(document.getElementById('root')).render(
  <StrictMode>
      <TodoApp />   {/* Hier eingefügt */}
      <hr />
     <Chess />
  </StrictMode>,
)
