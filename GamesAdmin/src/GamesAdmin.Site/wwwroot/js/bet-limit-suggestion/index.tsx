import "regenerator-runtime/runtime.js";
import ReactDOM from "react-dom";
import React from "react";
import SettingBetLimitForNewCurrency from "./components/SettingBetLimitForNewCurrency";

const e = React.createElement;

globalThis.addEventListener("showBetLimitSuggestion", (event) => {
    const domContainer = document.querySelector("#bet-limit-suggestion-root");
    if (domContainer) {
        const detail = (event as CustomEvent).detail as {
            currencies: {
                operatorName: string;
                operatorId: string;
                currencyCode: string;
                multiplier: number;
                enabled: boolean;
                groupName: string;
                groupId: string;
            }[];
            operators: {
                operatorName: string;
                operatorId: string;
            }[];
        };
        const currencyGroupFilter = document.querySelector("#currency-group-filter") as HTMLDivElement;
        const currencyNeedToAdd = document.querySelector("#currency-need-to-add") as HTMLDivElement;
        const currency = {
            operatorId: currencyNeedToAdd.dataset.operatorId || "",
            currencyId: currencyNeedToAdd.dataset.currencyId || "",
            currencyName: currencyNeedToAdd.dataset.currencyName || "",
            currencyCode: currencyNeedToAdd.dataset.currencyCode || "",
        };
        const currencyCode = currencyGroupFilter.dataset.currencyCode || "";
        const currencyMultiplier = Number(currencyGroupFilter.dataset.currencyMultiplier) || 1;
        const currencyGroup = Array.from(
            new Set(
                detail.currencies
                    .filter(
                        (currency) =>
                            currency.currencyCode === currencyCode && currency.multiplier === currencyMultiplier && currency.enabled
                    )
                    .map((currency) => ({
                        operatorName: currency.operatorName,
                        operatorId: currency.operatorId,
                        groupName: currency.groupName,
                        groupId: currency.groupId,
                    }))
            )
        );
        ReactDOM.render(
            e(SettingBetLimitForNewCurrency, {
                currencyNeedToAdd: currency,
                currencyGroups: currencyGroup,
                operators: detail.operators.map((operator) => operator.operatorName),
            }),
            domContainer
        );
    } else {
        console.error("Could not find element with ID 'bet-limit-suggestion-root'");
    }
});
