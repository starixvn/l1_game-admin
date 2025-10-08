import React, { useState } from "react";
import { ModalContainer } from "../Common/ModalContainer";
import GameComparison from "./GameComparison";
import { IntegrateNewCurrencyApi } from "../../IntegrateNewCurrencyApi";
import { CurrencyGroupGame } from "../../types";
import CurrencyGroupGameModal from "./CurrencyGroupGameModal";

export default function CurrencyGroupDetail({
    showDetailsModal,
    closeDetailModal,
    selectedSuggestion,
}: {
    showDetailsModal: boolean;
    closeDetailModal: () => void;
    selectedSuggestion: any;
}) {
    const [currencyGroupGames, setCurrencyGroupGames] = useState<CurrencyGroupGame[]>([]);
    const [showCurrencyGroupGamesModal, setShowCurrencyGroupGamesModal] = useState(false);
    const handleViewCurrencyGroupGames = () => {
        (async () => {
            const currencyGroupGames = await IntegrateNewCurrencyApi.getCurrencyGroupGamesByGroupId(selectedSuggestion.groupId);
            setCurrencyGroupGames(currencyGroupGames);
            setShowCurrencyGroupGamesModal(true);
        })();
    };
    return (
        <>
            <ModalContainer
                show={showDetailsModal}
                onClose={closeDetailModal}
                title={`Currency Group Detail`}
                maxWidth="600px"
                modalId="currency-group-detail-modal"
                overflowContent={true}
            >
                <>
                    <div className="suggestion-info">
                        <div className="suggestion-details">
                            <strong>Group Name:</strong> {selectedSuggestion.groupName}
                        </div>
                        <div className="suggestion-details">
                            <strong>Operator ID:</strong> {selectedSuggestion.partnerId}
                        </div>
                    </div>
                    <div className="my-3">
                        {selectedSuggestion.gameComparisons?.map((game: any) => (
                            <GameComparison game={game} />
                        ))}
                    </div>
                    <button onClick={() => handleViewCurrencyGroupGames()} className="suggestion-button">
                        View Currency Group Games
                    </button>
                </>
            </ModalContainer>

            {showCurrencyGroupGamesModal && (
                <CurrencyGroupGameModal
                    show={showCurrencyGroupGamesModal}
                    onClose={() => setShowCurrencyGroupGamesModal(false)}
                    title="Currency Group Games"
                    currencyGroupGames={currencyGroupGames}
                    modalId="currency-group-games-modal"
                />
            )}
        </>
    );
}
