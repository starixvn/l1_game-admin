import React, { useMemo, useState } from "react";
import { ModalContainer } from "../Common/ModalContainer";
import { CurrencyGroupGame } from "../../types";
import CurrencyGroupGameItem, { CurrencyGroupGameType } from "./CurrencyGroupGameItem";

/*
bbbb:
bit 1: coin value.length > 0
bit 2: bet level.length > 0
bit 3: max bet per choice.length > 0
*/

const currencyGroupGameTypeMapping = {
    4: CurrencyGroupGameType.CoinValue,
    2: CurrencyGroupGameType.BetLevel,
    1: CurrencyGroupGameType.MaxBetPerChoice,
    0: CurrencyGroupGameType.NoSpecialConfig,
};

export default function CurrencyGroupGameModal({
    show,
    onClose,
    title,
    currencyGroupGames,
    modalId,
}: {
    show: boolean;
    onClose: () => void;
    title: string;
    currencyGroupGames: CurrencyGroupGame[];
    modalId?: string;
}) {
    const [searchValue, setSearchValue] = useState("");
    const currencyGroupGameWithTypes = useMemo(() => {
        return currencyGroupGames.map((game) => {
            const hasCoinValue = Number(game.coinValues.length > 0);
            const hasBetLevel = Number(game.betLevels.length > 0);
            const hasMaxBetPerChoice = Number(Object.keys(game.maxBetPerChoices).length > 0);
            const bitmask = (hasCoinValue << 2) | (hasBetLevel << 1) | (hasMaxBetPerChoice << 0);
            return {
                ...game,
                type: currencyGroupGameTypeMapping[bitmask],
            };
        });
    }, []);

    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.stopPropagation();
        const value = e.target.value;
        setSearchValue(value);
    };

    return (
        <ModalContainer show={show} onClose={onClose} title={title} maxWidth="800px" modalId={modalId} overflowContent={true}>
            <>
                <div className="search-container">
                    <input id="search-input" onChange={handleSearch} type="text" placeholder="Search by game name" value={searchValue} />
                </div>
                <div className="mt-3">
                    {currencyGroupGameWithTypes
                        .filter((game) => game.gameName.toLowerCase().includes(searchValue.toLowerCase()))
                        .map((game) => (
                            <CurrencyGroupGameItem game={game} type={game.type} />
                        ))}
                </div>
            </>
        </ModalContainer>
    );
}
