export function piece(type, color, currentPos, specialboolean){
    return {type, color, currentPos, specialboolean};
}

function isSchach(currentBoard, color) {
    return false;
}


/// ROCHADE INFO: ÜBERSPRINGENDE FELDER NICHT IM SCHACH SEIN

export function validatingLegalityController (currentBoard, movingPiece, newPos, IsEnPassant){
    if(isSchach(currentBoard, movingPiece.color) ||
        (currentBoard[newPos]!=null && currentBoard[newPos].color===movingPiece.color)){
        return false;
    }
    const typeSpecificValidation = typeSpecificValdiationRules[movingPiece.type];
        if(typeSpecificValidation){
            return typeSpecificValidation(currentBoard, movingPiece, newPos);
        }

    return false;
}
const typeSpecificValdiationRules= {
    "B":(currentBoard, movingPiece, newPos,isEnPassant)=>{
        if(movingPiece.color==='w'){
            if(
                currentBoard[newPos]===null &&
                newPos === movingPiece.currentPos-8 ){
                return true;
            }
            if(
                (newPos=== movingPiece.currentPos-7 || newPos=== movingPiece.currentPos-9) &&
                ((currentBoard[newPos]===null && isEnPassant) ||
                    (currentBoard[newPos]!=null &&
                    currentBoard[newPos].color==='b'))
            ){
                return true;
            }
            if(movingPiece.specialboolean&& newPos===movingPiece.currentPos-16 &&
                (currentBoard[newPos]===null))
            {return true;}
        }
        if(movingPiece.color==='b'){

            if(
                currentBoard[newPos]===null &&
                newPos === movingPiece.currentPos+8 ){
                return true;
            }
            if(
                (newPos=== movingPiece.currentPos+7 || newPos=== movingPiece.currentPos+9) &&
                ((currentBoard[newPos]===null && isEnPassant) ||
                    (currentBoard[newPos]!=null &&
                        currentBoard[newPos].color==='w'))
            ){
                return true;
            }
            if(movingPiece.specialboolean&& newPos===movingPiece.currentPos+16 &&
                (currentBoard[newPos]===null))
            {return true;}
        }

        return false;
    },
    "T":(currentBoard, movingPiece, newPos)=>{},
}


