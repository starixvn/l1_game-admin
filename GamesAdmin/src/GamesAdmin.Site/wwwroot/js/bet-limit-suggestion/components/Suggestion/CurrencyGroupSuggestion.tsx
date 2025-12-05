import React, { useState, useEffect, useRef, useMemo } from "react";
import { StatusMessage } from "../Common/StatusMessage";
import { IntegrateNewCurrencyApi, CurrencyGroup } from "../../IntegrateNewCurrencyApi";
import MultiSelectDropdown from "../Common/MultiSelectDropDown";
import { TabbedSuggestionInterface, TabbedSuggestionInterfaceRef, SuggestionType } from "./TabbedSuggestionInterface";
import { ExistingRangeSuggestionResult, NewCurrencyGroupSuggestion } from "../../types";

interface CurrencyGroupSuggestionProps {
    operators: string[];
    defaultCurrencyGroupName: string;
    currencyGroups: { groupId: string; groupName: string; operatorId: string; operatorName: string }[];
    handleApplySelection: (selection: any) => void;
}

interface SuggestionFormData {
    currencyRate: number;
    baseCurrencyGroupId: string;
}

export function CurrencyGroupSuggestion({
    operators: partners,
    defaultCurrencyGroupName,
    currencyGroups,
    handleApplySelection,
}: CurrencyGroupSuggestionProps) {
    const [suggestionFormData, setSuggestionFormData] = useState<SuggestionFormData>({
        currencyRate: 0.6,
        baseCurrencyGroupId: "",
    });
    const tabbedSuggestionRef = useRef<TabbedSuggestionInterfaceRef>(null);
    useEffect(() => {
        const defaultGroup = currencyGroups.find((group) => group.groupName === defaultCurrencyGroupName);
        if (defaultGroup) {
            setSuggestionFormData((x) => ({
                ...x,
                baseCurrencyGroupId: defaultGroup.groupId,
            }));
        }
    }, [defaultCurrencyGroupName, currencyGroups]);

    const [error, setError] = useState("");
    const [selectedPartners, setSelectedPartners] = useState<string[]>([]);
    const [suggestions, setSuggestions] = useState<ExistingRangeSuggestionResult | null>(null);
    const [newRangeSuggestions, setNewRangeSuggestions] = useState<NewCurrencyGroupSuggestion | null>(null);

    const validateExistingSuggestion = () => {
        if (selectedPartners.length === 0) {
            setError("Please select at least one partner.");
            return false;
        }
        return validateNewRangeSuggestion();
    };

    const validateNewRangeSuggestion = () => {
        if (!suggestionFormData.baseCurrencyGroupId) {
            setError("Please select a base currency group for suggestions.");
            return false;
        }
        if (suggestionFormData.currencyRate <= 0) {
            setError("Exchange rate must be greater than 0.");
            return false;
        }
        return true;
    };

    const handleSuggestExistingGroup = () => {
        if (!validateExistingSuggestion()) {
            return;
        }
        (async () => {
            try {
                const result = await IntegrateNewCurrencyApi.getBetLimitSuggestion({
                    baseCurrencyGroupId: suggestionFormData.baseCurrencyGroupId,
                    currencyRate: suggestionFormData.currencyRate,
                    candidateCurrencyGroupIds: currencyGroups
                        .filter((x) => selectedPartners.includes(x.operatorName))
                        .map((x) => x.groupId),
                });

                setSuggestions(result);

                tabbedSuggestionRef.current?.setActiveTab(SuggestionType.Existing);
                setError("");
            } catch (error) {
                console.error("Error getting existing group suggestions:", error);
                setError("Failed to get existing group suggestions. Please try again.");
            }
        })();
    };

    const handleSuggestNewRange = () => {
        if (!validateNewRangeSuggestion()) {
            return;
        }
        (async () => {
            try {
                const suggestions = await IntegrateNewCurrencyApi.getNewRangeBetLimitSuggestion({
                    baseCurrencyGroupId: suggestionFormData.baseCurrencyGroupId,
                    currencyRate: suggestionFormData.currencyRate,
                    candidateCurrencyGroupIds: selectedPartners,
                });

                setNewRangeSuggestions(suggestions);
                tabbedSuggestionRef.current?.setActiveTab(SuggestionType.NewRange);
                setError("");
            } catch (error) {
                console.error("Error getting new range suggestions:", error);
                setError("Failed to get new range suggestions. Please try again.");
            }
        })();
    };

    const hasSuggestions = useMemo(() => suggestions || newRangeSuggestions, [suggestions, newRangeSuggestions]);

    return (
        <>
            <div className="form">
                <div className="form-group row">
                    <div className="col-sm-12">
                        <label htmlFor="currencyRate">Exchange Rate to Base Currency *</label>
                        <input
                            className="form-control"
                            id="currencyRate"
                            type="number"
                            step="0.0001"
                            min="0.0001"
                            value={suggestionFormData.currencyRate}
                            onChange={(e) => setSuggestionFormData((x) => ({ ...x, currencyRate: parseFloat(e.target.value) }))}
                        />
                    </div>
                </div>
                <div className="form-group row">
                    <div className="col-sm-12">
                        <label htmlFor="baseCurrencyGroupId">Base Currency Group for Suggestions *</label>
                        <select
                            className="form-control"
                            value={suggestionFormData.baseCurrencyGroupId}
                            id="baseCurrencyGroupId"
                            onChange={(e) => setSuggestionFormData((x) => ({ ...x, baseCurrencyGroupId: e.target.value }))}
                        >
                            <option value="">Select Base Currency Group for Suggestions</option>
                            {currencyGroups.map((group) => (
                                <option key={group.groupId} value={group.groupId}>
                                    {group.groupName} ({group.operatorId})
                                </option>
                            ))}
                        </select>
                    </div>
                </div>
            </div>

            <MultiSelectDropdown
                options={partners.map((partner) => ({ label: partner, value: partner }))}
                onChange={(value) => setSelectedPartners(value)}
                title="Operators to include in suggestions"
            ></MultiSelectDropdown>

            <button
                type="button"
                className="btn-add"
                onClick={() => handleSuggestExistingGroup()}
                disabled={!suggestionFormData.currencyRate}
            >
                Suggest Existing Group
            </button>
            <button
                type="button"
                className="btn-add m-4"
                onClick={() => handleSuggestNewRange()}
                disabled={!suggestionFormData.currencyRate}
            >
                Suggest New Range
            </button>

            <StatusMessage error={error} />

            {hasSuggestions && (
                <TabbedSuggestionInterface
                    ref={tabbedSuggestionRef}
                    existingSuggestions={suggestions}
                    newRangeSuggestions={newRangeSuggestions}
                    onApplySelection={handleApplySelection}
                />
            )}
        </>
    );
}
