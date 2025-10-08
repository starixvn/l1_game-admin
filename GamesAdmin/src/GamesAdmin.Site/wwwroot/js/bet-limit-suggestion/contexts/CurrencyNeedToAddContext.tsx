import React from "react";

export const CurrencyNeedToAddContext = React.createContext<{
    currencyNeedToAdd: {
        operatorId: string;
        currencyId: string;
        currencyName: string;
        currencyCode: string;
    };
}>({ currencyNeedToAdd: { operatorId: "", currencyId: "", currencyName: "", currencyCode: "" } });

interface CurrencyNeedToAddContextProps {
    currencyNeedToAdd: {
        operatorId: string;
        currencyId: string;
        currencyName: string;
        currencyCode: string;
    };
}

export const CurrencyNeedToAddContextProvider: React.FC<CurrencyNeedToAddContextProps> = ({ currencyNeedToAdd, children }) => {
    return <CurrencyNeedToAddContext.Provider value={{ currencyNeedToAdd }}>{children}</CurrencyNeedToAddContext.Provider>;
};

export const useCurrencyNeedToAdd = () => {
    return React.useContext(CurrencyNeedToAddContext);
};
