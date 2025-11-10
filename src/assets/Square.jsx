export function Square({ index, value, onSquareClick, playerColor }){
    const row = Math.floor(index / 8);
    const col = index % 8;
    // Correct chessboard coloring: A8 (row 0, col 0) is dark for White view
    const baseIsDark = (row + col) % 2 === 0;
    // For Black view, invert the colors so the pattern is reversed from their perspective
    const isDark = playerColor === 'B' ? !baseIsDark : baseIsDark;

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