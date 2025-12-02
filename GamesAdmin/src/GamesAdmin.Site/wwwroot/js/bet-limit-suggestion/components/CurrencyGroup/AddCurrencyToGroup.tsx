import React, { useState } from "react";
import { ModalContainer } from "../Common/ModalContainer";
import { ExistingCurrencyGroupSuggestion } from "../../types";
import { useCurrencyNeedToAdd } from "../../contexts/CurrencyNeedToAddContext";
import { StatusMessage } from "../Common/StatusMessage";

export const AddCurrencyToGroup = ({
    show,
    onClose,
    title,
    selectedSuggestion,
}: {
    show: boolean;
    onClose: () => void;
    title: string;
    selectedSuggestion: ExistingCurrencyGroupSuggestion | null;
}) => {
    const { currencyNeedToAdd } = useCurrencyNeedToAdd();
    const [error, setError] = useState<string>("");
    const handleAddCurrencyToGroup = () => {
        console.log("handleAddCurrencyToGroup");
        setError("Please select a target currency group.");
    };
    const handleCloneGroup = () => {
        console.log("handleCloneGroup");
        setError("Please select a target currency group.");
    };
    return (
        <ModalContainer show={show} onClose={onClose} title={title} maxWidth="600px" modalId="add-currency-to-group-modal">
            <>
                <div className="form">
                    <div className="form-group row">
                        <div className="col-sm-12">
                            <label htmlFor="currencyName">Currency Name*</label>
                            <input className="form-control" id="currencyRate" type="text" value={currencyNeedToAdd.currencyName} disabled />
                        </div>
                    </div>
                    <div className="form-group row">
                        <div className="col-sm-12">
                            <label htmlFor="baseCurrencyGroupId">Operator *</label>
                            <input className="form-control" id="operatorId" type="text" value={currencyNeedToAdd.operatorId} disabled />
                        </div>
                    </div>
                    <div className="section">
                        <h3>Target Currency Group</h3>
                        <ul>
                            <li>Name: {selectedSuggestion?.groupName}</li>
                            <li>Operator: {selectedSuggestion?.partnerId}</li>
                        </ul>
                    </div>
                </div>

                {selectedSuggestion?.partnerId !== currencyNeedToAdd.operatorId && (
                    <div className="section">
                        <h3>OperatorId mismatch, you need clone this group to your operator</h3>
                        <div className="form">
                            <div className="form-group row">
                                <div className="col-sm-12">
                                    <label htmlFor="name">New Currency Group Name *</label>
                                    <input className="form-control" id="name" type="text" defaultValue={selectedSuggestion?.groupName} />
                                </div>
                            </div>
                        </div>
                        <button type="button" className="btn-add" onClick={() => handleCloneGroup()} disabled={!selectedSuggestion}>
                            Clone Group
                        </button>
                    </div>
                )}

                <button type="button" className="btn-add" onClick={() => handleAddCurrencyToGroup()} disabled={!selectedSuggestion}>
                    Add Currency to Group
                </button>
                <StatusMessage error={error} />
            </>
        </ModalContainer>
    );
};
