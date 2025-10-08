import React from "react";
import { CurrencyGroupGame } from "../../types";

export enum CurrencyGroupGameType {
    MaxBetPerChoice = "maxBetPerChoice",
    CoinValue = "coinValue",
    BetLevel = "betLevel",
    NoSpecialConfig = "noSpecialConfig",
}

export default function CurrentCurrencyGroupGameItem({ game, type }: { game: CurrencyGroupGame; type: CurrencyGroupGameType }) {
    return (
        <div className="suggestion-item">
            <h4>
                {game.gameName} (ID: {game.gameId})
            </h4>
            <ul>
                {type === CurrencyGroupGameType.MaxBetPerChoice && (
                    <>
                        <li>Chips: {game.chips?.join(", ")}</li>
                        <li>Min Bet: {game.minBet}</li>
                        <li>Max Bet: {game.maxBet}</li>
                        <li>
                            <div>Max Bet Per Choice</div>
                            <ul>
                                {Object.entries(game.maxBetPerChoices).map(([key, value]) => (
                                    <li>
                                        {key}: {value}
                                    </li>
                                ))}
                            </ul>
                        </li>
                    </>
                )}
                {type === CurrencyGroupGameType.CoinValue && (
                    <>
                        <li>Default Coin Value: {game.defaultCoinValue}</li>
                        <li>Coin Value</li>
                        {game.coinValues?.join(", ")}
                    </>
                )}
                {type === CurrencyGroupGameType.BetLevel && (
                    <>
                        <li>Default Bet Level: {game.defaultBetLevel}</li>
                        <li>Bet Level</li>
                        {game.betLevels?.join(", ")}
                    </>
                )}
                {type === CurrencyGroupGameType.NoSpecialConfig && (
                    <>
                        <li>Chips: {game.chips?.join(", ")}</li>
                        <li>Min Bet: {game.minBet}</li>
                        <li>Max Bet: {game.maxBet}</li>
                    </>
                )}
            </ul>
        </div>
    );
}
