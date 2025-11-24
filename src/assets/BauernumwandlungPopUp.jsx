import React from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import './Pieces.jsx'
import * as Pieces from "./Pieces.jsx";

export default function BauernumwandlungPopUp({ isOpen, onClose, trigger, onSelect, color, tone }) {
    const normalizedColor = (color || '').toString().toLowerCase();
    const figures = [
        { piece: Pieces.piece('D', normalizedColor, 0) },
        { piece: Pieces.piece('P', normalizedColor, 0) },
        { piece: Pieces.piece('S', normalizedColor, 0) },
        { piece: Pieces.piece('T', normalizedColor, 0) },
    ];

    return (
        <Popup
            open={isOpen}
            onClose={onClose}
            trigger={trigger}
            position={["top center", "right center", "bottom center", "left center"]}
            arrow={true}
            closeOnDocumentClick={false}
            keepTooltipInside="body"
            repositionOnResize
        >
            <div className={`promotion-menu promotion-menu--tone-${tone}`}>
                <div className="promotion-grid">
                {figures.map((fig) => (
                    <button
                        className="promotion-btn"
                        key={fig.piece.type}
                        onClick={() => onSelect(fig.piece.type)}
                    >
                        <span className={`promotion-piece square--color-${normalizedColor}`}>
                            {fig.piece.type}
                        </span>
                    </button>
                ))}
                </div>
            </div>
        </Popup>
    );
}