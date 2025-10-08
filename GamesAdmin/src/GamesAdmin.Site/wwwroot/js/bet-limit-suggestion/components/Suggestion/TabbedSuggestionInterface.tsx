import React, { useState, useEffect, useImperativeHandle, forwardRef } from "react";
import { CurrencyGroupGame, ExistingRangeSuggestionResult, ExistingCurrencyGroupSuggestion, NewCurrencyGroupSuggestion } from "../../types";
import { NewRangeSuggestion } from "./NewRangeSuggestion";
import { ExistingGroupSuggestion } from "./ExistingGroupSuggestion";

export interface SuggestionSelection {
    type: SuggestionType;
    data: ExistingCurrencyGroupSuggestion | CurrencyGroupGame[];
}

export enum SuggestionType {
    Existing = "existing",
    NewRange = "new-range",
}

interface TabbedSuggestionInterfaceProps {
    existingSuggestions: ExistingRangeSuggestionResult | null;
    newRangeSuggestions: NewCurrencyGroupSuggestion | null;
    onApplySelection: (selection: any) => void;
}
export interface TabbedSuggestionInterfaceRef {
    setActiveTab: (tab: SuggestionType) => void;
}

export const TabbedSuggestionInner = (
    { existingSuggestions, newRangeSuggestions, onApplySelection }: TabbedSuggestionInterfaceProps,
    ref: React.Ref<TabbedSuggestionInterfaceRef>
) => {
    const [activeTab, setActiveTab] = useState<SuggestionType>(SuggestionType.Existing);

    useImperativeHandle(ref, () => ({
        setActiveTab,
    }));

    useEffect(() => {
        if (existingSuggestions && !newRangeSuggestions) {
            setActiveTab(SuggestionType.Existing);
        } else if (!existingSuggestions && newRangeSuggestions) {
            setActiveTab(SuggestionType.NewRange);
        } else if (existingSuggestions && newRangeSuggestions) {
            if (activeTab !== SuggestionType.Existing && activeTab !== SuggestionType.NewRange) {
                setActiveTab(SuggestionType.Existing);
            }
        }
    }, [existingSuggestions, newRangeSuggestions]);

    if (!existingSuggestions && !newRangeSuggestions) {
        console.log("TabbedSuggestionInterface: No suggestions, returning null");
        return null;
    }

    return (
        <div className="tabs-container">
            <div className="tabs-header">
                {existingSuggestions && (
                    <button
                        className={`tab-button ${activeTab === SuggestionType.Existing ? "active" : ""}`}
                        onClick={() => setActiveTab(SuggestionType.Existing)}
                    >
                        Existing Groups
                    </button>
                )}
                {newRangeSuggestions && (
                    <button
                        className={`tab-button ${activeTab === SuggestionType.NewRange ? "active" : ""}`}
                        onClick={() => setActiveTab(SuggestionType.NewRange)}
                    >
                        New Range
                    </button>
                )}
            </div>

            <div className="mt-3 tab-content">
                {existingSuggestions && (
                    <div className={`tab-panel ${activeTab === SuggestionType.Existing ? "active" : ""}`}>
                        <ExistingGroupSuggestion
                            suggestions={existingSuggestions}
                            onAddCurrency={(suggestion) => onApplySelection({ type: "existing", data: suggestion })}
                        />
                    </div>
                )}

                {newRangeSuggestions && (
                    <div className={`tab-panel ${activeTab === SuggestionType.NewRange ? "active" : ""}`}>
                        <NewRangeSuggestion suggestions={newRangeSuggestions} />
                    </div>
                )}
            </div>
        </div>
    );
};

export const TabbedSuggestionInterface = forwardRef(TabbedSuggestionInner);
