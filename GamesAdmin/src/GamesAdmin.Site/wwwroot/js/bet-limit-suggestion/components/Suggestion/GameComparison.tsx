import React from "react";
export default function GameComparison({ game }: { game: any }) {
    const renderBetLimitComparison = (comparison: any, label: string) => {
        if (!comparison || comparison.baseValue === 0) return <></>;
        return (
            <div className="suggestion-item">
                <h4>{label}</h4>
                <div>Candidate: {comparison.candidateValue}</div>
                <div>Expected: {comparison.expectedValue?.toFixed(2)}</div>
            </div>
        );
    };
    return (
        <div className="suggestion-item">
            <h4>
                {game.gameDisplayName} (ID: {game.gameId})
            </h4>

            <div style={{ display: "flex", justifyContent: "flex-start", gap: "0.8rem" }}>
                {renderBetLimitComparison(game.minBetComparison, "Min Bet")}
                {renderBetLimitComparison(game.maxBetComparison, "Max Bet")}
                {renderBetLimitComparison(game.maxBetPerChoiceComparison, "Max Bet Per Choice")}
                {renderBetLimitComparison(game.minCoinValueComparison, "Min Coin Value")}
                {renderBetLimitComparison(game.maxCoinValueComparison, "Max Coin Value")}
            </div>
        </div>
    );
}
