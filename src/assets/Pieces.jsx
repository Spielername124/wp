export function piece(type, color, currentPos, specialboolean){
    return {type, color, currentPos, specialboolean};
}

function isSchach(currentBoard, color) {
    return false;
}


/// ROCHADE INFO: ÜBERSPRINGENDE FELDER NICHT IM SCHACH SEIN

export function validatingLegalityController (currentBoard, lastBoard, movingPiece, newPos){
    if(newPos==movingPiece.currentPos||isSchach(currentBoard, movingPiece.color) ||
        (currentBoard[newPos]!=null && currentBoard[newPos].color===movingPiece.color)){
        return false;
    }
    if(newPos>=64)return false;
    const typeSpecificValidation = typeSpecificValdiationRules[movingPiece.type];
        if(typeSpecificValidation){
            return typeSpecificValidation(currentBoard, lastBoard, movingPiece, newPos);
        }

    return false;
}
const typeSpecificValdiationRules= {
    "B":(currentBoard, lastBoard, movingPiece, newPos)=>{
        const enPassant = isEnPassant(currentBoard, lastBoard, movingPiece, newPos);
        const movingRow = Math.trunc(movingPiece.currentPos / 8);
        const targetRow = Math.trunc(newPos / 8);
        if(movingPiece.color==='w'){
            if(
                currentBoard[newPos]===null &&
                newPos === movingPiece.currentPos-8 ){
                return true;
            }
            if(
                (movingRow-1===targetRow&&(newPos=== movingPiece.currentPos-7 || newPos=== movingPiece.currentPos-9)) &&
                ((currentBoard[newPos]===null && enPassant) ||
                    (currentBoard[newPos]!=null &&
                    currentBoard[newPos].color==='b'))
            ){
                return true;
            }
            if(movingPiece.specialboolean&& newPos===movingPiece.currentPos-16 &&
                (currentBoard[newPos]===null) && currentBoard[newPos+8]===null)
            {return true;}
        }
        if(movingPiece.color==='b'){

            if(
                currentBoard[newPos]===null &&
                newPos === movingPiece.currentPos+8 ){
                return true;
            }
            if(
                (movingRow+1=== targetRow && (newPos=== movingPiece.currentPos+7 || newPos=== movingPiece.currentPos+9)) &&
                ((currentBoard[newPos]===null && enPassant) ||
                    (currentBoard[newPos]!=null &&
                        currentBoard[newPos].color==='w'))
            ){
                return true;
            }
            if(movingPiece.specialboolean&& newPos===movingPiece.currentPos+16 &&
                (currentBoard[newPos]===null) && currentBoard[newPos-8]===null)
            {return true;}
        }

        return false;
    },
    "T":(currentBoard, lastBoard, movingPiece, newPos)=>{
        return straightLineValidation(currentBoard, movingPiece, newPos);
    },
    'S':  (currentBoard, lastBoard, movingPiece, newPos)=>{
        return diagonalLineValidation(currentBoard, movingPiece, newPos);
    },
    'D':  (currentBoard, lastBoard, movingPiece, newPos)=>{
        return diagonalLineValidation(currentBoard, movingPiece, newPos) ||
            straightLineValidation(currentBoard, movingPiece, newPos);
    },
    'K':  (currentBoard, lastBoard, movingPiece, newPos)=>{
        const movingRow = Math.trunc(movingPiece.currentPos / 8);
        const targetRow = Math.trunc(newPos / 8);
        const movingCol = movingPiece.currentPos % 8;
        const targetCol = newPos % 8;
        if(Math.abs(movingRow-targetRow)<=1 && Math.abs(movingCol-targetCol)<=1)return true;
        return false;
    },
    'P':  (currentBoard, lastBoard, movingPiece, newPos)=>{
        const movingRow = Math.trunc(movingPiece.currentPos / 8);
        const targetRow = Math.trunc(newPos / 8);
        const movingCol = movingPiece.currentPos % 8;
        const targetCol = newPos % 8;
        const rowDiff = Math.abs(movingRow-targetRow);
        const colDiff = Math.abs(movingCol-targetCol);
        if((rowDiff===2 && colDiff===1) || (rowDiff===1 && colDiff===2) ){
            return true;
        }
        return false;
    }


}
function diagonalLineValidation(currentBoard, movingPiece, newPos){
    const movingRow = Math.trunc(movingPiece.currentPos / 8);
    const movingCol = movingPiece.currentPos % 8;
    const targetRow = Math.trunc(newPos / 8);
    const targetCol = newPos % 8;
    const rowDiff = Math.abs(movingRow-targetRow);
    const colDiff = Math.abs(movingCol-targetCol);

    const smallerPiece = Math.min(movingPiece.currentPos, newPos);
    const biggerPiece = Math.max(movingPiece.currentPos, newPos);

    if(rowDiff!==colDiff){return false;}

    if((biggerPiece-smallerPiece)%9===0){
        const delta = (biggerPiece-smallerPiece)/9;
        for(let i= 1; i<delta; i++){
          if(currentBoard[smallerPiece+9*i]!==null){
              return false;
          }
        }
        return true;
    }
    if((biggerPiece-smallerPiece)%7===0){
        const delta = (biggerPiece-smallerPiece)/7;
        for(let i= 1; i<delta; i++){
            if(currentBoard[smallerPiece+7*i]!==null){
                return false;
            }
        }
        return true;
    }


    return false;
}
function straightLineValidation(currentBoard, movingPiece, newPos){
    const movingRow = Math.trunc(movingPiece.currentPos / 8);
    const movingCol = movingPiece.currentPos % 8;
    const targetRow = Math.trunc(newPos / 8);
    const targetCol = newPos % 8;

    if(movingRow===targetRow){
        for(let i= Math.min(movingCol,targetCol)+1; i<Math.max(movingCol,targetCol); i++){
            if(currentBoard[movingRow*8+i]!==null){
                return false;
            }
        }
        return true;
    }
    else if(movingCol===targetCol){
        for(let i= Math.min(movingRow,targetRow)+1; i<Math.max(movingRow,targetRow); i++){
            if(currentBoard[movingCol+i*8]!==null){
                return false;
            }
        }
        return true;
    }

    return false;

}

export function isEnPassant(currentBoard, lastBoard, movingPiece, newPos){
    if(lastBoard===null)return false;
    if(newPos+8>=64||newPos-8 <0)return false;
    if(movingPiece.currentPos===newPos+8|| movingPiece.currentPos===newPos-8)return false;
    if (movingPiece.color==='w' && movingPiece.type === 'B' &&
        lastBoard[newPos-8]!=null&& currentBoard[newPos+8] != null &&
        (lastBoard[newPos-8].type === 'B' && currentBoard[newPos] === null && lastBoard[newPos+8] === null && currentBoard[newPos+8].type === 'B')){
        return true;
    }
    if(movingPiece.color==='b' && movingPiece.type === 'B' &&
        lastBoard[newPos+8]!=null&& currentBoard[newPos-8] != null &&
        (lastBoard[newPos+8].type === 'B' && currentBoard[newPos] === null && lastBoard[newPos-8] === null && currentBoard[newPos-8].type === 'B')){
        return true;
    }
    return false;
}



