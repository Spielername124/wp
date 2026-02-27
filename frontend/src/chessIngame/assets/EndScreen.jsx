import React, { useEffect } from 'react';
import { createPortal } from 'react-dom';

export default function EndScreen({ open, onClose, onRematch, title, message }) {
    useEffect(() => {
        if (!open) return;
        const onKey = (e) => {
            if (e.key === 'Escape') {
                onClose?.();
            }
        };
        window.addEventListener('keydown', onKey);
        return () => window.removeEventListener('keydown', onKey);
    }, [open, onClose]);

    if (!open) return null;

    const container = document.querySelector('.board-overlay-wrap');
    const overlay = (
        <div className="end-local-overlay" onClick={() => onClose?.()}>
            <div className="end-modal" onClick={(e) => e.stopPropagation()}>
                <button
                    className="end-modal__close"
                    aria-label="close"
                    onClick={() => onClose?.()}
                >
                    ×
                </button>

                <div className="end-modal__body">
                    {title && <h3 className="end-modal__title">{title}</h3>}
                    {message && <p className="end-modal__message">{message}</p>}

                    <div className="end-modal__actions">
                        <button
                            className="btn btn--primary"
                            onClick={onRematch}
                        >
                            Rematch
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );

    return container ? createPortal(overlay, container) : overlay;
}