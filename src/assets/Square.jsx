export function Square({ index, value, onSquareClick }){
    const row = Math.floor(index / 8);
    const col = index % 8;
    const isDark = (row + col) % 2 === 1;
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