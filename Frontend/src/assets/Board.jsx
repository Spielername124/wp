import {useState} from "react";
import { piece} from "./Pieces.jsx";
import {isEnPassant, validatingLegalityController} from "./Logic/moveValidation.jsx"
import {noPossibleMoves, isSchach} from "./Logic/GameLogic.jsx"
import {Square} from "./Square.jsx";


export function Board({lastSquares, squares, onPlay, readOnly,setBadge,resetBadges,playerColor,onGameEnd}) {
    const [chosenPiece, setChosenPiece] = useState(null);
    const [pendingPromotion, setPendingPromotion] = useState(null);

    const isWhite = playerColor=== 'W'
    function showIndicators(number) {
        //TODO: Make more efficient if bored
        for (let i = 0; i < 64; i++) {
            if (validatingLegalityController(squares, lastSquares, squares[number], i)) {
                setBadge(i, "X");
            }
        }
    }

    function handleClick(number) {

        //restricting movement while promotion is pending or readOnly
        if (pendingPromotion) return;
        if (readOnly) return;
        let nextSquares = squares.slice();

        //Do nothing if square is empty and no piece is chosen
        if (squares[number] === null && chosenPiece === null) {
            resetBadges(nextSquares);
            return;
        }
        //restricting choosing a opposing piece as moving piece
        if(chosenPiece===null&&((squares[number].color==='w'&& !isWhite)|| squares[number].color==='b'&& isWhite)){
            resetBadges();
            return;
        }
        //chose Piece and show indicators
        if (chosenPiece == null) {
            setChosenPiece(number)
            showIndicators(number);
            setBadge(number, "A");
            return;
        }
        //canceling selection, removing indicators
        else if (chosenPiece === number) {
            setChosenPiece(null);
            resetBadges();
            return;
        }
        //Validating move and playing it
        else if (validatingLegalityController(nextSquares, lastSquares, squares[chosenPiece], number)) {
            const targetRow = Math.floor(number / 8);

            // Promotion-Logik
            if(squares[chosenPiece].type==='B' && ((isWhite&&targetRow===0)||!isWhite&&targetRow===7)){
                setPendingPromotion({ sourceIndex: chosenPiece, targetIndex: number });
                return;
            }
            //En passant special logic
            if (lastSquares !== null && (squares[chosenPiece].type === 'B' && isEnPassant(nextSquares, lastSquares, squares[chosenPiece], number))) {
                if (squares[chosenPiece].color === 'w') {
                    nextSquares[number + 8] = null;
                } else {
                    nextSquares[number - 8] = null;
                }
            }
            //rochade special logic
            if (squares[chosenPiece].type === 'K') {
                if (number === chosenPiece + 2) {
                    nextSquares[number - 1] = piece(squares[number + 1].type, squares[number + 1].color, number - 1);
                    nextSquares[number + 1] = null;
                } else if (number === chosenPiece - 2) {
                    nextSquares[number + 1] = piece(squares[number - 2].type, squares[number - 2].color, number + 1);
                    nextSquares[number - 2] = null;
                }
            }
            //common move logic
            nextSquares[number] = piece(squares[chosenPiece].type, squares[chosenPiece].color, number);
            nextSquares[squares[chosenPiece].currentPos] = null;
            resetBadges();
            setChosenPiece(null);

            //Logic if no moves for the next player is poosible -> remis or winn

            const nextToMove = isWhite? 'b': 'w';
            if(noPossibleMoves(nextSquares,squares,nextToMove)){
                if(isSchach(nextSquares,squares, nextToMove, false)){
                    onGameEnd({type:'s', winner: nextSquares[number].color});
                }
                else {
                    onGameEnd({type:'d'});
                }
            }
            //moving on
            onPlay(nextSquares);

            //invalid move --> do nothing but unselect piece
        } else {
            setChosenPiece(null);
            resetBadges();
        }

    }

    function handlePromotionSelect(type) {
        if(!pendingPromotion) return;
        const { sourceIndex, targetIndex } = pendingPromotion;

        let nextSquares = squares.slice();
        nextSquares[sourceIndex] = null;
        nextSquares[targetIndex] = piece(type, squares[sourceIndex].color, targetIndex);
        setChosenPiece(null);
        setPendingPromotion(null);
        resetBadges();

        const nextToMove = isWhite? 'b': 'w';
        if(noPossibleMoves(nextSquares,squares,nextToMove)){
            if(isSchach(nextSquares,squares, nextToMove, false)){
                onGameEnd({type:'s', winner: nextToMove=== 'w'? 'b': 'w'});
            }
            else {
                onGameEnd({type:'d'});
            }
        }

        onPlay(nextSquares);
    }

    // helper to not duplicate code while displaing the board depending on playerColor
    const renderSquare = (i, j) => {
        const idx = i * 8 + j;
        return (
            <Square
                key={j}
                index={idx}
                value={squares[idx]}
                onSquareClick={() => handleClick(idx)}
                playerColor={playerColor}
                isInPromotion={pendingPromotion?.targetIndex === idx}
                onPromotionSelect={handlePromotionSelect}
            />
        );
    };

    if (playerColor === 'W') {
        return (
            <>
                {Array.from({length: 8}, (_, i) => (
                    <div key={i} className="board-row">
                        {Array.from({length: 8}, (_, j) => renderSquare(i, j))}
                    </div>
                ))}
            </>
        );
    }
    if(playerColor==='B') {
        return (
            <>
                {Array.from({length: 8}, (_, originalRowIndex) => 7 - originalRowIndex).map(i => (
                    <div key={i} className="board-row">
                        {Array.from({length: 8}, (_, originalColIndex) => 7 - originalColIndex).map(j =>
                            renderSquare(i, j)
                        )}
                    </div>
                ))}
            </>
        );
    }
}