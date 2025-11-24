import {useState} from "react";
import {isEnPassant, isSchachMatt, piece, validatingLegalityController} from "./Pieces.jsx";
import * as Pieces from "./Pieces.jsx";
import {Square} from "./Square.jsx";


export function Board({lastSquares, squares, onPlay, readOnly,setBadge,resetBadges,playerColor}) {
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
        else if (Pieces.validatingLegalityController(nextSquares, lastSquares, squares[chosenPiece], number)) {
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
                    nextSquares[number - 1] = Pieces.piece(squares[number + 1].type, squares[number + 1].color, number - 1);
                    nextSquares[number + 1] = null;
                } else if (number === chosenPiece - 2) {
                    nextSquares[number + 1] = Pieces.piece(squares[number - 2].type, squares[number - 2].color, number + 1);
                    nextSquares[number - 2] = null;
                }
            }
            //common move logic
            nextSquares[number] = Pieces.piece(squares[chosenPiece].type, squares[chosenPiece].color, number);
            nextSquares[squares[chosenPiece].currentPos] = null;
            resetBadges();
            setChosenPiece(null);
            //nuking the field if other side has no valid moves (nuking for debugging reasons) but is only schachmatt if it is schach an no valid moves.
            //Implementet like this is a remis schachmatt to.
            if(isSchachMatt(nextSquares,squares,'b')|| isSchachMatt(nextSquares,squares,'w')){
                nextSquares = Array(64).fill(null);
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

        // Alte Figur weg
        nextSquares[sourceIndex] = null;

        // Neue Figur hin (Farbe übernehmen)
        const color = squares[sourceIndex].color;
        nextSquares[targetIndex] = piece(type, color, targetIndex);

        // Aufräumen
        setChosenPiece(null);
        setPendingPromotion(null); // Popup schließen
        resetBadges();

        // Schachmatt prüfen (optional, je nach deiner Logik)
        if(isSchachMatt(nextSquares, squares, 'b') || isSchachMatt(nextSquares, squares, 'w')){
            // nextSquares = Array(64).fill(null); // Deine Reset Logik
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