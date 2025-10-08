import React from "react";

export default function ExistingSuggestionItem({
    suggestion,
    handleViewDetail,
    handleAddCurrencyToGroup,
}: {
    suggestion: any;
    handleViewDetail: (suggestion: any) => void;
    handleAddCurrencyToGroup: (suggestion: any) => void;
}) {
    return (
        <div key={suggestion.groupId} className={`suggestion-item`}>
            <div className="suggestion-content">
                <div className="suggestion-info">
                    <div className="suggestion-details">
                        <strong>Group:</strong> {suggestion.groupName}
                    </div>
                    <div className="suggestion-details">
                        <strong>Operator:</strong> {suggestion.partnerId}
                    </div>
                </div>

                <div className="suggestion-actions">
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            handleViewDetail(suggestion);
                        }}
                        className="suggestion-button primary"
                    >
                        View Detail
                    </button>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            handleAddCurrencyToGroup(suggestion);
                        }}
                        className="suggestion-button primary"
                    >
                        Add to this group
                    </button>
                </div>
            </div>
        </div>
    );
}
