export function BadgesLayer({ badges, playerColour, playerColor }) {
    // Accept both spellings, prefer the one actually provided by parent
    const color = playerColor ?? playerColour;

    const getOrderedIndices = () => {
        const orderedIndices = [];

        if (color === 'B') {
            // Black perspective: render H8..A1 so overlay aligns with reversed board
            for (let i = 7; i >= 0; i--) {
                for (let j = 7; j >= 0; j--) {
                    orderedIndices.push(i * 8 + j);
                }
            }
        } else {
            // White perspective: A8..H1 natural order
            for (let i = 0; i < 8; i++) {
                for (let j = 0; j < 8; j++) {
                    orderedIndices.push(i * 8 + j);
                }
            }
        }

        return orderedIndices;
    };

    const orderedIndices = getOrderedIndices();

    return (
        <div className="badges-layer">
            {orderedIndices.map(idx => (
                <span
                    key={idx}
                    className="badge"
                >
                    {badges[idx] ?? ''}
                </span>
            ))}
        </div>
    );
}