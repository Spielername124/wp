import {useState} from "react";
import {isEnPassant, validatingLegalityController} from "./Pieces.jsx";
import * as Pieces from "./Pieces.jsx";
import {Square} from "./Square.jsx";


export function Board({isNext, lastSquares, squares, onPlay, readOnly,setBadge,resetBadges}){
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
            if(lastSquares!== null && (squares[chosenPiece].type==='B' && isEnPassant(nextSquares, lastSquares, squares[chosenPiece], number))){
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