import React, { useEffect, useState } from "react";
import { CurrencyGroupGame, NewCurrencyGroupSuggestion } from "../../types";
import CurrentCurrencyGroupGameItem, { CurrencyGroupGameType } from "./CurrencyGroupGameItem";
import CurrencyGroupGameModal from "./CurrencyGroupGameModal";

interface NewRangeSuggestionProps {
    suggestions: NewCurrencyGroupSuggestion;
}

export function NewRangeSuggestion({ suggestions }: NewRangeSuggestionProps) {
    const { maxBetPerChoiceGames, coinValueGames, betLevelGames, noSpecialConfigGames } = suggestions;
    useEffect(() => {
        console.log(suggestions);
    }, [suggestions]);
    return (
        <>
            <button className="suggestion-button mb-4">Use new range</button>
            <div className="new-range-container">
                {maxBetPerChoiceGames.length > 0 && (
                    <ExampleGameItem
                        games={maxBetPerChoiceGames}
                        type={CurrencyGroupGameType.MaxBetPerChoice}
                        title="Used Max Bet Per Choice Games"
                    />
                )}
                {coinValueGames.length > 0 && (
                    <ExampleGameItem games={coinValueGames} type={CurrencyGroupGameType.CoinValue} title="Used Coin Value Games" />
                )}
                {betLevelGames.length > 0 && (
                    <ExampleGameItem games={betLevelGames} type={CurrencyGroupGameType.BetLevel} title="Used Bet Level Games" />
                )}
                {noSpecialConfigGames.length > 0 && (
                    <ExampleGameItem
                        games={noSpecialConfigGames}
                        type={CurrencyGroupGameType.NoSpecialConfig}
                        title="Used Min Bet and Max Bet Games"
                    />
                )}
            </div>
        </>
    );
}

function ExampleGameItem({ games, type, title }: { games: CurrencyGroupGame[]; type: CurrencyGroupGameType; title: string }) {
    const [showMore, setShowMore] = useState(false);
    return (
        <div className="suggestion-item">
            <h4 className="mb-3" style={{ fontWeight: "bold" }}>
                {title}
            </h4>
            <div className="new-range-item-content">
                <CurrentCurrencyGroupGameItem game={games[0]} type={type} />
            </div>
            <button className="suggestion-button" onClick={() => setShowMore(true)}>
                Show More
            </button>
            {showMore && (
                <CurrencyGroupGameModal show={showMore} onClose={() => setShowMore(false)} title={title} currencyGroupGames={games} />
            )}
        </div>
    );
}
