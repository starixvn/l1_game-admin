import React, { useEffect, useRef, useState } from "react";
import ReactDOM from "react-dom";

interface ModalContainerProps {
    show: boolean;
    onClose: () => void;
    children: React.ReactNode;
    title: string;
    maxWidth?: string;
    modalId?: string;
    overflowContent?: boolean;
}

export function ModalContainer({
    show,
    onClose,
    children,
    title,
    maxWidth = "600px",
    modalId,
    overflowContent = false,
}: ModalContainerProps) {
    const modalRef = useRef<HTMLDivElement>(null);
    const overlayRef = useRef<HTMLDivElement>(null);
    const currentModalId = modalId || `modal-${Date.now()}-${Math.random()}`;

    useEffect(() => {
        if (!show) return;

        const handleClickOutside = (event: MouseEvent) => {
            const target = event.target as HTMLElement;
            if (modalRef.current && !modalRef.current.contains(target)) {
                onClose();
            }
        };
        overlayRef.current?.addEventListener("mousedown", handleClickOutside);
        return () => {
            overlayRef.current?.removeEventListener("mousedown", handleClickOutside);
        };
    }, [show, onClose, currentModalId]);

    if (!show) {
        return null;
    }

    return ReactDOM.createPortal(
        <div
            className={`sub-modal ${overflowContent ? "overflow-content" : ""}`}
            style={{
                display: show ? "block" : "none",
                zIndex: 1050,
            }}
            ref={overlayRef}
        >
            <div
                className="modal-dialog"
                style={{
                    maxWidth,
                    zIndex: 1051,
                }}
                ref={modalRef}
            >
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 style={{ fontSize: "1.6rem" }} className="modal-title">
                            {title}
                        </h5>
                        <button type="button" className="close" data-dismiss="modal" aria-label="Close" onClick={onClose}>
                            <span style={{ fontSize: "2rem" }} aria-hidden="true">
                                &times;
                            </span>
                        </button>
                    </div>
                    <div className="modal-body">{children}</div>
                </div>
            </div>
        </div>,
        document.querySelector("#content>.inner-content") as HTMLElement
    );
}
