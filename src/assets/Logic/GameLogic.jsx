import {validatingLegalityController} from "./moveValidation.jsx";

function searchKing(currentBoard, color){
    for(let i=0; i<64; i++){
        if(currentBoard[i]!== null && currentBoard[i].color === color && currentBoard[i].type === 'K')return i;
    }
    return null;
}
export function isSchach(currentBoard, lastBoard, color, checkingNextTurnSchach=false) {
    const kingPos = searchKing(currentBoard, color);
    for (let i = 0; i < 64; i++) {
        if (currentBoard[i] !== null && currentBoard[i].color !== color) {
            if (validatingLegalityController(currentBoard, lastBoard, currentBoard[i], kingPos, checkingNextTurnSchach)) {
                return true;
            }
        }
    }
    return false;
}
export function noPossibleMoves(squares, lastSquares, color){
    for(let i=0; i<64; i++) {
        if (squares[i] !== null && squares[i].color === color && canMove(squares, lastSquares, i)) return false;
    }
    return true;
}
function canMove(squares, lastSquares, number){
    for(let i=0; i<64; i++){
        if(validatingLegalityController(squares, lastSquares, squares[number], i))return true;
    }
    return false;
}