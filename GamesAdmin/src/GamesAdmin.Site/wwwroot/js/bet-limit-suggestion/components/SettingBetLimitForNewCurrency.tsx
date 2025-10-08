import React, { useState } from "react";
import { StatusMessage } from "./Common/StatusMessage";
import { CurrencyGroupSuggestion } from "./Suggestion/CurrencyGroupSuggestion";
import { SuggestionSelection } from "./Suggestion/TabbedSuggestionInterface";
import { ModalContainer } from "./Common/ModalContainer";
import { CurrencyNeedToAddContext, CurrencyNeedToAddContextProvider } from "../contexts/CurrencyNeedToAddContext";

interface SettingBetLimitForNewCurrencyProps {
    operators: string[];
    currencyGroups: {
        operatorName: string;
        operatorId: string;
        groupName: string;
        groupId: string;
    }[];
    currencyNeedToAdd: {
        operatorId: string;
        currencyId: string;
        currencyName: string;
        currencyCode: string;
    };
}

export default function SettingBetLimitForNewCurrency({
    operators,
    currencyGroups,
    currencyNeedToAdd,
}: SettingBetLimitForNewCurrencyProps) {
    const [success, setSuccess] = useState<string>("");
    const [error, setError] = useState<string>("");
    const [showModal, setShowModal] = useState<boolean>(true);

    const handleApplySelection = (selection: SuggestionSelection) => {};

    const handleClose = () => {
        setShowModal(false);
    };

    if (!showModal) {
        return null;
    }

    return (
        <ModalContainer
            show={showModal}
            onClose={handleClose}
            title="Currency Group Suggestion"
            maxWidth="600px"
            modalId="currency-group-suggestion-modal"
        >
            <StatusMessage success={success} error={error} />
            <CurrencyNeedToAddContextProvider currencyNeedToAdd={currencyNeedToAdd}>
                <CurrencyGroupSuggestion
                    operators={operators}
                    defaultCurrencyGroupName="Cash_Asia"
                    currencyGroups={currencyGroups}
                    handleApplySelection={handleApplySelection}
                />
            </CurrencyNeedToAddContextProvider>
        </ModalContainer>
    );
}
