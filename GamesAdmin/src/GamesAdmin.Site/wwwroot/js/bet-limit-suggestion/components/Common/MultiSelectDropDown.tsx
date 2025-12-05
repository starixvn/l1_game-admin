import React, { useEffect, useState, useRef } from "react";

interface Option {
    value: string;
    label: string;
}

const MultiSelectDropdown = ({
    options,
    onChange,
    value,
    title,
    selectAllByDefault = true,
}: {
    options: Option[];
    onChange: (values: string[]) => void;
    value?: string[];
    title?: string;
    selectAllByDefault?: boolean;
}) => {
    const isInitializedRef = useRef(false);
    const onChangeRef = useRef(onChange);
    onChangeRef.current = onChange;

    const getInitialSelectedItems = () => {
        if (value !== undefined) {
            return value;
        }
        if (selectAllByDefault && options.length > 0) {
            return options.map((option) => option.value);
        }
        return [];
    };

    const [selectedItems, setSelectedItems] = useState<string[]>(getInitialSelectedItems);
    const [selectAll, setSelectAll] = useState(false);
    const [isOpen, setIsOpen] = useState(false);
    const dropdownRef = useRef<HTMLDivElement>(null);

    const handleToggle = () => {
        setIsOpen(!isOpen);
    };

    const handleSelectAll = () => {
        if (selectAll) {
            setSelectedItems([]);
        } else {
            setSelectedItems(options.map((option) => option.value));
        }
    };

    // Update selectAll state when selectedItems or options change
    useEffect(() => {
        setSelectAll(selectedItems.length === options.length && options.length > 0);
    }, [selectedItems, options.length]);

    // Notify parent of changes (skip initial mount to avoid double-calling)
    useEffect(() => {
        if (isInitializedRef.current) {
            onChangeRef.current(selectedItems);
        } else {
            isInitializedRef.current = true;
            // Call onChange on mount to sync initial state with parent
            onChangeRef.current(selectedItems);
        }
    }, [selectedItems]);

    // Handle controlled value changes from parent
    useEffect(() => {
        if (value !== undefined) {
            setSelectedItems(value);
        }
    }, [value]);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
                setIsOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    const handleCheckboxChange = (value: string) => {
        if (selectedItems.includes(value)) {
            setSelectedItems(selectedItems.filter((selected) => selected !== value));
        } else {
            setSelectedItems([...selectedItems, value]);
        }
    };

    const handleRemoveItem = (valueToRemove: string) => {
        setSelectedItems(selectedItems.filter((value) => value !== valueToRemove));
    };

    const getSelectedLabels = () => {
        return selectedItems.map((value) => {
            const option = options.find((opt) => opt.value === value);
            return option ? option.label : value;
        });
    };

    return (
        <div ref={dropdownRef} style={{ position: "relative", width: "100%" }}>
            {title && <label>{title}</label>}
            <div
                className="multi-select-dropdown"
                onClick={handleToggle}
                onMouseEnter={(e) => {
                    e.currentTarget.style.borderColor = "#007bff";
                }}
                onMouseLeave={(e) => {
                    e.currentTarget.style.borderColor = "#ccc";
                }}
            >
                {selectedItems.length > 0 ? (
                    <div className="dropdown-container">
                        {getSelectedLabels().map((label) => {
                            const value = options.find((opt) => opt.label === label)?.value || label;
                            return (
                                <span key={value} className="dropdown-item-label">
                                    {label}
                                    <span
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleRemoveItem(value);
                                        }}
                                        className="dropdown-item-label-remove-icon"
                                        title="Remove item"
                                    >
                                        ×
                                    </span>
                                </span>
                            );
                        })}
                    </div>
                ) : (
                    <span style={{ color: "#999" }}>Select operators...</span>
                )}
            </div>
            {isOpen && (
                <div className="dropdown-list-container">
                    {options.length === 0 ? (
                        <div style={{ padding: "10px", color: "#999", textAlign: "center" }}>No operators available</div>
                    ) : (
                        <>
                            <div
                                key={"all"}
                                className="dropdown-list-item"
                                onMouseEnter={(e) => {
                                    if (!selectedItems.includes("all")) {
                                        e.currentTarget.style.backgroundColor = "#f0f0f0";
                                    }
                                }}
                                onMouseLeave={(e) => {
                                    if (!selectedItems.includes("all")) {
                                        e.currentTarget.style.backgroundColor = "#fff";
                                    }
                                }}
                            >
                                <label className="dropdown-list-item-label">
                                    <input
                                        type="checkbox"
                                        checked={selectAll}
                                        onChange={(e) => {
                                            handleSelectAll();
                                        }}
                                        style={{ marginRight: "8px" }}
                                    />
                                    Select All
                                </label>
                            </div>
                            {options.map((option) => (
                                <div
                                    key={option.value}
                                    className="dropdown-list-item"
                                    onMouseEnter={(e) => {
                                        if (!selectedItems.includes(option.value)) {
                                            e.currentTarget.style.backgroundColor = "#f0f0f0";
                                        }
                                    }}
                                    onMouseLeave={(e) => {
                                        if (!selectedItems.includes(option.value)) {
                                            e.currentTarget.style.backgroundColor = "#fff";
                                        }
                                    }}
                                >
                                    <label className="dropdown-list-item-label">
                                        <input
                                            type="checkbox"
                                            checked={selectedItems.includes(option.value)}
                                            onChange={(e) => {
                                                handleCheckboxChange(option.value);
                                            }}
                                            style={{ marginRight: "8px" }}
                                        />
                                        {option.label}
                                    </label>
                                </div>
                            ))}
                        </>
                    )}
                </div>
            )}
        </div>
    );
};

export default MultiSelectDropdown;
