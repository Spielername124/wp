import {useState} from "react";
import * as Pieces from "./Pieces.jsx";
import {isEnPassant, piece, validatingLegalityController} from "./Pieces.jsx";

/*

P=Horsey
b=bauer
t=turm
s=springer
d=dame
k=könig


*/


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
        initial[i+8]=Pieces.piece('B', 'b', i+8);
        initial[i+6*8]=Pieces.piece('B', 'w', i+6*8);
    }
    initial[56]=Pieces.piece('T', 'w',56,true);
    initial[57]=Pieces.piece('P', 'w',57);
    initial[58]=Pieces.piece('S', 'w',58);
    initial[59]=Pieces.piece('D', 'w',59);
    initial[60]=Pieces.piece('K', 'w',60,true);
    initial[61]=Pieces.piece('S', 'w',61);
    initial[62]=Pieces.piece('P', 'w',62);
    initial[63]=Pieces.piece('T', 'w',63,true);

    return initial
}
export default function Game(){
    const [history, setHistory] = useState(()=> [Restart()]);
    const [currentMove, setCurrentMove] = useState(0);
    const lastSquares = currentMove>0?history[currentMove-1]:null;
    const currentSquares = history[currentMove];
    const isNext = currentMove % 2 === 0;
    const isNotHistory = currentMove === history.length - 1;
    const [badges, setBadges] = useState(() => Array(64).fill(null));

    function setBadge(index, text){
        setBadges(prev => {
            const copy = prev.slice();
            copy[index] = text ?? null;
            return copy;
        });
    }
    function resetBadges(){
        setBadges(prev => {
            return Array(64).fill(null);
        });
    }


    function handlePlay(nextSquares){
        const nextHistory =  [...history.slice(0, currentMove + 1),nextSquares];
        setHistory(nextHistory);
        setCurrentMove(nextHistory.length - 1);

    }

    function jumpTo(nextMove) {
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

    return  (
        <div className="game">
            <div className="game-board">
                <div className="board-overlay-wrap">
                    <Board isNext={isNext}
                           squares={currentSquares}
                           lastSquares={lastSquares}
                           onPlay={handlePlay}
                           readOnly={!isNotHistory}
                           setBadge={setBadge}
                           resetBadges={resetBadges}
                    />

                    <div className="badges-layer">
                        {Array.from({ length: 64 }, (_, idx) => (
                            <span key={idx} className="badge">{badges[idx] ?? ''}</span>
                        ))}
                    </div>
                </div>
            </div>
            <div className="game-info">
                <ol>{moves}</ol>
            </div>
        </div>
    );
}
function Square({ index, value, onSquareClick }){
    const row = Math.floor(index / 8);
    const col = index % 8;
    const isDark = (row + col) % 2 === 1;
    const classNames = [
        'square',
        isDark ? 'square--dark' : 'square--light',
        value && value.color ? `square--color-${value.color}` : ''
    ].join(' ').trim();

    return(
        <button
            className={classNames}
            onClick={onSquareClick}>
            {value ? value.type : null}
        </button>
    )
}
//TODO


function Board({isNext, lastSquares, squares, onPlay, readOnly,setBadge,resetBadges}){
    const[chosenPiece, setChosenPiece] = useState(null);

    function showIndicators(number){
        //TODO: Make more efficient if bored
        for(let i=0; i<64; i++){
            if(validatingLegalityController(squares, lastSquares,squares[number], i)){
                setBadge(i, "X");
            }
        }
    }

    function handleClick(number){
        if(readOnly)return;
        const nextSquares = squares.slice();
        if(squares[number]==null&&chosenPiece==null){
            return;
        }
        if(chosenPiece==null){
            setChosenPiece(number)
            showIndicators(number);
            setBadge(number, "A");
            return;
        }
        else if(chosenPiece===number){
            setChosenPiece(null);
            resetBadges();
            return;
        }
        else if(Pieces.validatingLegalityController(nextSquares, lastSquares, squares[chosenPiece], number)){
            if(lastSquares!== null && (chosenPiece.type==='B' && isEnPassant(nextSquares, lastSquares, squares[chosenPiece], number))){
                if(squares[chosenPiece].color==='w'){
                    nextSquares[number+8] = null;
                }
                else{
                    nextSquares[number-8] = null;
                }
            }
            nextSquares[number] = Pieces.piece(squares[chosenPiece].type, squares[chosenPiece].color, number);
            nextSquares[squares[chosenPiece].currentPos] = null;
            resetBadges();
            setChosenPiece(null);
            onPlay(nextSquares);}
        else{
            setChosenPiece(null);
            resetBadges();
        }

    }

    return(
        <>
            {Array.from({length: 8}, (_, i) => (
                <div key={i} className="board-row">
                    {Array.from({length: 8}, (_, j) => (
                        <Square
                            key={j}
                            index={i * 8 + j}
                            value={squares[i * 8 + j]}
                            onSquareClick={() => handleClick(i * 8 + j)}
                        />
                    ))}
                </div>
            ))}
        </>
    );
}