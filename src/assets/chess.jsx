import {useState} from "react";
import * as Pieces from "./Pieces.jsx";

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

    initial[0] = Pieces.piece('T', 'b', 0);
    initial[1] = Pieces.piece('P', 'b',1);
    initial[2] = Pieces.piece('S', 'b',2);
    initial[3] = Pieces.piece('D', 'b',3);
    initial[4] = Pieces.piece('K', 'b',4);
    initial[5] = Pieces.piece('S', 'b',5);
    initial[6] = Pieces.piece('P', 'b',6);
    initial[7] = Pieces.piece('T', 'b',7);

    for(let i=0; i<8; i++){
        initial[i+8]=Pieces.piece('B', 'b', i+8);
        initial[i+6*8]=Pieces.piece('B', 'w', i+6*8);
    }
    initial[56]=Pieces.piece('T', 'w',56);
    initial[57]=Pieces.piece('P', 'w',57);
    initial[58]=Pieces.piece('S', 'w',58);
    initial[59]=Pieces.piece('D', 'w',59);
    initial[60]=Pieces.piece('K', 'w',60);
    initial[61]=Pieces.piece('S', 'w',61);
    initial[62]=Pieces.piece('P', 'w',62);
    initial[63]=Pieces.piece('T', 'w',63);

    return initial;
}
export default function Game(){
    const [history, setHistory] = useState(()=> [Restart()]);
    const [currentMove, setCurrentMove] = useState(0);
    const currentSquares = history[currentMove];
    const isNext = currentMove % 2 === 0;

    function handlePlay(nextSquares){
        const nextHistory =  [...history.slice(0, currentMove + 1), nextSquares];
        setHistory(nextHistory);
        setCurrentMove(nextHistory.length - 1);

    }

    return  (
        <div className="game">
            <div className="game-board">
                <Board isNext={isNext} squares={currentSquares} onPlay={handlePlay} chosenPiece={null}/>
            </div>
        </div>
    );
}
function Square({value,onSquareClick}){
    return(
        <button
            className="square"
            onClick = {onSquareClick}>
            {value ? value.type : null}
        </button>
    )
}
function Board({isNext, squares, onPlay, chosenPiece}){
    function handleClick(number){
        const nextSquares = squares.slice();
        if(squares[number]==null&&chosenPiece==null){
            onPlay(nextSquares);
        }
        if(chosenPiece==null){
            chosenPiece = squares[number];
        }
        else{
            nextSquares[number] = Pieces.piece(chosenPiece.type, chosenPiece.color, number);
            nextSquares[chosenPiece.currentPos] = null;
            chosenPiece = null;
            onPlay(nextSquares);}

    }

    return(
        <>
            <div> "text" </div>
            {Array.from({length: 8}, (_, i) => (
            <div key={i} className="board-row">
                {Array.from({length: 8}, (_, j) => (
                    <Square key={j} value={squares[i * 8 + j]} onSquareClick={() =>handleClick(i * 8 + j)} />
                ))}
            </div>
            ))}
        </>
    );
}