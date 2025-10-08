import React from "react";

interface StatusMessageProps {
    error?: string;
    success?: string;
}

export function StatusMessage({ error, success }: StatusMessageProps) {
    if (error) {
        return (
            <div
                style={{
                    marginBottom: "15px",
                    padding: "10px",
                    backgroundColor: "#f8d7da",
                    color: "#721c24",
                    borderRadius: "4px",
                    border: "1px solid #f5c6cb",
                }}
            >
                {error}
            </div>
        );
    }

    if (success) {
        return (
            <div
                style={{
                    marginBottom: "15px",
                    padding: "10px",
                    backgroundColor: "#d4edda",
                    color: "#155724",
                    borderRadius: "4px",
                    border: "1px solid #c3e6cb",
                }}
            >
                {success}
            </div>
        );
    }

    return null;
}
