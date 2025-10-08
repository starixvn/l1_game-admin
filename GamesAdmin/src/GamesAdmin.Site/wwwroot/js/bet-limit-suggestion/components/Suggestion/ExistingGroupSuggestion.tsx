import React, { useState } from "react";
import { ExistingCurrencyGroupSuggestion, ExistingRangeSuggestionResult } from "../../types";
import CurrencyGroupDetail from "./CurrencyGroupDetail";
import ExistingSuggestionItem from "./ExistingSuggestionItem";
import { AddCurrencyToGroup } from "../CurrencyGroup/AddCurrencyToGroup";

interface ExistingGroupSuggestionProps {
    suggestions: ExistingRangeSuggestionResult;
    onAddCurrency: (suggestion: ExistingCurrencyGroupSuggestion) => void;
}

export const ExistingGroupSuggestion: React.FC<ExistingGroupSuggestionProps> = ({ suggestions, onAddCurrency }) => {
    const [selectedSuggestion, setSelectedSuggestion] = useState<ExistingCurrencyGroupSuggestion | null>(null);
    const [showDetailsModal, setShowDetailsModal] = useState(false);
    const [showAddCurrencyToGroupModal, setShowAddCurrencyToGroupModal] = useState(false);

    const existingCount = suggestions
        ? (suggestions.bestSuggestion ? 1 : 0) +
          (suggestions.bestSmallerSuggestion && suggestions.bestSmallerSuggestion.groupId !== suggestions.bestSuggestion?.groupId ? 1 : 0)
        : 0;

    const handleViewDetail = (suggestion: ExistingCurrencyGroupSuggestion) => {
        setSelectedSuggestion(suggestion);
        setShowDetailsModal(true);
    };

    const handleAddCurrencyToGroup = () => {
        setShowAddCurrencyToGroupModal(true);
    };

    const closeDetailModal = () => {
        setShowDetailsModal(false);
    };

    const closeAddCurrencyToGroupModal = () => {
        setShowAddCurrencyToGroupModal(false);
    };

    return (
        <div className="suggestions-container">
            {existingCount < 1 && (
                <div className="suggestion-header-section">
                    <div>
                        <div className="suggestion-subtitle">No suggestions available</div>
                    </div>
                </div>
            )}

            <div className="suggestions-list">
                {suggestions.bestSuggestion && (
                    <ExistingSuggestionItem
                        suggestion={suggestions.bestSuggestion}
                        handleAddCurrencyToGroup={handleAddCurrencyToGroup}
                        handleViewDetail={handleViewDetail}
                    />
                )}
                {suggestions.bestSmallerSuggestion && suggestions.bestSmallerSuggestion.groupId !== suggestions.bestSuggestion?.groupId && (
                    <ExistingSuggestionItem
                        suggestion={suggestions.bestSmallerSuggestion}
                        handleAddCurrencyToGroup={handleAddCurrencyToGroup}
                        handleViewDetail={handleViewDetail}
                    />
                )}
            </div>

            {selectedSuggestion && (
                <CurrencyGroupDetail
                    showDetailsModal={showDetailsModal}
                    closeDetailModal={closeDetailModal}
                    selectedSuggestion={selectedSuggestion}
                />
            )}
            {selectedSuggestion && (
                <AddCurrencyToGroup
                    selectedSuggestion={selectedSuggestion}
                    show={showAddCurrencyToGroupModal}
                    onClose={closeAddCurrencyToGroupModal}
                    title="Add Currency to Group"
                />
            )}
        </div>
    );
};
