import {useState} from "react";
import * as Pieces from "./Pieces.jsx";
import {Board} from "./Board.jsx";
import {BadgesLayer} from "./Badges.jsx";

function Restart(){
    const initial = Array(64).fill(null);

    initial[0] = Pieces.piece('T', 'b', 0,true);
    initial[1] = Pieces.piece('P', 'b',1);
    initial[2] = Pieces.piece('S', 'b',2);
    initial[3] = Pieces.piece('D', 'b',3);
    initial[4] = Pieces.piece('K', 'b',4,true);
    initial[5] = Pieces.piece('S', 'b',5);
    initial[6] = Pieces.piece('P', 'b',6);
    initial[7] = Pieces.piece('T', 'b',7,true);

    for(let i=0; i<8; i++){
        initial[i+8]=Pieces.piece('B', 'b', i+8,true);
        initial[i+6*8]=Pieces.piece('B', 'w', i+6*8,true);
    }
    initial[56]=Pieces.piece('T', 'w',56,true);
    initial[57]=Pieces.piece('P', 'w',57);
    initial[58]=Pieces.piece('S', 'w',58);
    initial[59]=Pieces.piece('D', 'w',59);
    initial[60]=Pieces.piece('K', 'w',60,true);
    initial[61]=Pieces.piece('S', 'w',61);
    initial[62]=Pieces.piece('P', 'w',62);
    initial[63]=Pieces.piece('T', 'w',63,true);
    initial[50]=Pieces.piece('B', 'b', 50,true);

    return initial
}
export default function Game(){
    const [history, setHistory] = useState(()=> [Restart()]);
    const [currentMove, setCurrentMove] = useState(0);
    const lastSquares = currentMove>0?history[currentMove-1]:null;
    const currentSquares = history[currentMove];
    const isNotHistory = currentMove === history.length - 1;
    const [badges, setBadges] = useState(() => Array(64).fill(null));
    const [playerColor, setPlayerColor] = useState('W');

    function setBadge(index, text){
        setBadges(prev => {
            const copy = prev.slice();
            copy[index] = text ?? null;
            return copy;
        });
    }
    function resetBadges(){
        setBadges(() => {
            return Array(64).fill(null);
        });
    }



    function handlePlay(nextSquares){
        const nextHistory =  [...history.slice(0, currentMove + 1),nextSquares];
        setHistory(nextHistory);
        setCurrentMove(nextHistory.length - 1);
        if(playerColor === 'W'){
            setPlayerColor('B');
        }
        else{
            setPlayerColor('W');
        }

    }

    function jumpTo(nextMove) {
        resetBadges();
        setCurrentMove(nextMove);
    }
    const moves = history.map((squares, move) =>{
        let description;
        if (move > 0) {
            description = 'Go to move #' + move;
        } else {
            description = 'Go to game start';
        }
        return(
            <li key={move}>
                <button onClick={() => jumpTo(move)} >{description} </button>
            </li>
        );
    });
//TODO: Bauerumwanlung, REMIS von Schachmatt unterscheiden.
    return  (
        <div className="game">
            <div className="game-board">
                <div className="board-overlay-wrap">
                    <Board squares={currentSquares}
                           lastSquares={lastSquares}
                           onPlay={handlePlay}
                           readOnly={!isNotHistory}
                           setBadge={setBadge}
                           resetBadges={resetBadges}
                           playerColor={playerColor}
                    />

                    <BadgesLayer badges={badges} playerColour={playerColor}/>
                </div>
            </div>
            <div className="game-info">
                <ol>{moves}</ol>
            </div>
        </div>
    );
}




