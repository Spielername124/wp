import React from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';

export default function BauernumwandlungPopUp({ isOpen, onClose, trigger, onSelect }) {
    const figures = [
        {type: 'D'},
        {type: 'P'},
        {type: 'S'},
        {type: 'T'},

    ];

    return (
        <Popup
            open={isOpen}
            onClose={onClose}
            trigger={trigger}
            position = "top center"
            arrow={true}
            closeOnDocumentClick={false}
        ><div className="promotion-menu" style={{ padding: '10px', textAlign: 'center', background: 'white', border: '1px solid #ccc' }}>
            <h4>Wähle Figur:</h4>
            <div style={{ display: 'flex', gap: '5px' }}>
                {figures.map((fig) => (
                    <button
                        key={fig.type}
                        onClick={() => onSelect(fig.type)}
                        style={{ fontSize: '1.5rem', cursor: 'pointer', padding: '5px' }}
                    >
                        {fig.label}
                    </button>
                ))}
            </div>
        </div>
        </Popup>
    );

}