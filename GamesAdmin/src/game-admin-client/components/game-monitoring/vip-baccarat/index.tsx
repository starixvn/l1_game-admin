import React from "react";

import BaccaratModel from "../../common/model/BaccaratModel";
import { GameState } from "../../shared/common/GameState";
import Baccarat from "../baccarat";

const TOTAL_TIME = 30;
const WAITING_TIME = 15;

export default class VipBaccarat extends Baccarat {
    handleMessage(message) {
        super.handleMessage(message);
        
        if (!!message.inGame) {
            this.handleBet(message.inGame);
        }
    }

    handleStop(data) {
        this.isFirstLoad = false;
        const { result, settlementResult } = data;

        this.setState({
            result: { bankerCards: Object.values(result.player), playerCards: Object.values(result.banker) },
            settlementResult: settlementResult,
            gameState: GameState.waiting,
            time: {
                remainingTime: WAITING_TIME
            }
        });

        setTimeout(() => {
            this.setState({
                time: {
                    remainingTime: TOTAL_TIME
                },
                result: new BaccaratModel().result,
                settlementResult: {},
                roundNumber: this.state.roundNumber + 1,
                gameState: GameState.running
            });
        }, (WAITING_TIME + 1) * 1000);
    }
}