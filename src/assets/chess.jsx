import {useState} from "react";
import * as Pieces from "./Pieces.jsx";
import {Board} from "./Board.jsx";
import {BadgesLayer} from "./Badges.jsx";
import EndScreen from "./EndScreen.jsx";

//TODO: Ligtht up the chosen square instead of showing A as indicator

function Restart(){
    const initial = Array(64).fill(null);

    initial[0] = Pieces.piece('T', 'b', 0,true);
    initial[1] = Pieces.piece('P', 'b',1);
    initial[2] = Pieces.piece('L', 'b',2);
    initial[3] = Pieces.piece('D', 'b',3);
    initial[4] = Pieces.piece('K', 'b',4,true);
    initial[5] = Pieces.piece('L', 'b',5);
    initial[6] = Pieces.piece('P', 'b',6);
    initial[7] = Pieces.piece('T', 'b',7,true);

    for(let i=0; i<8; i++){
        initial[i+8]=Pieces.piece('B', 'b', i+8,true);
        initial[i+6*8]=Pieces.piece('B', 'w', i+6*8,true);
    }
    initial[56]=Pieces.piece('T', 'w',56,true);
    initial[57]=Pieces.piece('P', 'w',57);
    initial[58]=Pieces.piece('L', 'w',58);
    initial[59]=Pieces.piece('D', 'w',59);
    initial[60]=Pieces.piece('K', 'w',60,true);
    initial[61]=Pieces.piece('L', 'w',61);
    initial[62]=Pieces.piece('P', 'w',62);
    initial[63]=Pieces.piece('T', 'w',63,true);

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
    const [endOpen, setEndOpen] = useState(false);
    const [endTitle, setEndTitle] = useState('');
    const [endMessage, setEndMessage] = useState('');

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
            description = 'move ' + move;
        } else {
            description = 'initial';
        }
        return(
            <li key={move}>
                <button onClick={() => jumpTo(move)} >{description} </button>
            </li>
        );
    });
//TODO: Popup for after the game finished
    function handleGameEnd({ type, winner }) {
        if (type === 'd') {
            setEndTitle('Remis');
            setEndMessage('Die Partie endet unentschieden.');
        } else if (type === 's') {
            setEndTitle('Schachmatt');
            setEndMessage(winner === 'w' ? 'Weiß gewinnt!' : 'Schwarz gewinnt!');
        } else {
            setEndTitle('Spiel beendet');
            setEndMessage('');
        }
        setEndOpen(true);
    }

    function handleRematch() {
        setHistory([Restart()]);
        setCurrentMove(0);
        setPlayerColor('W');
        resetBadges();
        setEndOpen(false);
    }


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
                           onGameEnd={handleGameEnd}
                    />

                    <BadgesLayer badges={badges} playerColour={playerColor}/>
                </div>
            </div>
            <div className="game-info">
                <ol className="moves-list">{moves}</ol>
                <div className="game-info__actions">
                    <button className="btn btn--primary" onClick={handleRematch}>Rematch</button>
                </div>
            </div>
            <EndScreen
                open={endOpen}
                onClose={() => setEndOpen(false)}
                onRematch={handleRematch}
                title={endTitle}
                message={endMessage}
            />
        </div>
    );
}




