import {useState} from "react";

/*
Horsey weiss = 0
Bauer weiss = 1
Springer weiss = 2
Turm weiss = 3
Dame weiss = 4
König weiss = 5

Horsey schwarz = 6
Bauer schwarz = 7
Springer schwarz = 8
Turm schwarz = 9
Dame schwarz=10
König schwarz = 11


*/

function Restart(){
    const initial = Array(64).fill(null);

    initial[0] = 'X';
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
                <Board isNext={isNext} squares={currentSquares} onPlay={handlePlay}/>
            </div>
        </div>
    );
}
function Square({value,onSquareClick}){
    return(
        <button
            className="square"
            onClick = {onSquareClick}>
            {value}
        </button>
    )
}
function Board({isNext, squares, onPlay}){
    function handleClick(number){
        const nextSquares = squares.slice();
        if(squares[number]==null){
            if(isNext) {
                nextSquares[number] = 'X';
            }
            else{
                nextSquares[number] = 'O';
            }
            onPlay(nextSquares);
        }

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