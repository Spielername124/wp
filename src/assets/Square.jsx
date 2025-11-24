import BauernumwandlungPopUp from "./BauernumwandlungPopUp.jsx";

export function Square({ index, value, onSquareClick, playerColor, isInPromotion, onPromotionSelect}){
    const row = Math.floor(index / 8);
    const col = index % 8;
    const baseIsDark = (row + col) % 2 === 0;
    const isDark = playerColor === 'B' ? !baseIsDark : baseIsDark;

    const classNames = [
        'square',
        isDark ? 'square--dark' : 'square--light',
        value && value.color ? `square--color-${value.color}` : ''
    ].join(' ').trim();

    const squareButton = (
        <button
            className={classNames}
            onClick={onSquareClick}>
            {value ? value.type : null}
        </button>
    )
    if(isInPromotion){
        return (
            <BauernumwandlungPopUp
                trigger={squareButton}
                isOpen={true}
                onSelect={onPromotionSelect}
                color={playerColor}
                tone={isDark ? 'dark' : 'light'}
            />
        );
    }
    return squareButton;
}