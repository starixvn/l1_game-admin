import React from 'react';
import styles from './index.module.scss';
import BaccaratModel from '../../common/model/BaccaratModel';
import { GameState } from '../../shared/common/GameState';
import BetItem from '../../common/model/BetItem';

type Props = {
    onMessage: Function,
    sendMessage: Function,
    isProduction: boolean
};

type State = {
    data: BaccaratModel,
    gameState: number;
    isShowResultUpdater: boolean
};

const TOTAL_TIME = 30;
const WAITING_TIME = 9;
const TYPES = new Array("player", "banker", "tie", "playerPair", "bankerPair", "big", "small");

export default class Baccarat extends React.Component<Props, State>{
    constructor(props: Props) {
        super(props);

        this.state = {
            data: new BaccaratModel(),
            gameState: GameState.none,
            isShowResultUpdater: false
        };
    }

    componentDidMount() {
        this.props.onMessage((message) => this.handleMessage(message));
    }

    handleMessage(message) {
        if (message.remainingTime !== undefined) {
            this.handleStart(message);
        }
        else if (message.finish !== undefined) {
            this.handleStop(message);
        }
        else {
            this.handleBet(message);
        }
    }

    handleStart(message) {
        const newData = new BaccaratModel();
        let isFinished = message.remainingTime > TOTAL_TIME;
        newData.time.remainingTime = isFinished ? message.remainingTime - TOTAL_TIME : message.remainingTime;
        newData.roundNumber = isFinished ? message.roundId - 1 : message.roundId;

        this.setState({
            data: newData,
            gameState: isFinished ? GameState.waiting : GameState.running
        });

        if (isFinished) {
            this.countDownToStart.call(this, newData, newData.time.remainingTime + 1);
        }
    }

    handleBet(message) {
        const _this = this;
        const newData = new BaccaratModel();
        newData.roundNumber = this.state.data.roundNumber;

        Object.keys(message.ingame).forEach(function (key) {
            const userObj = message.ingame[key];
            TYPES.forEach(type => {
                if (userObj[type] > 0) {
                    newData[type].dataList.unshift(_this.getBet(type, userObj));
                }
            });
        });

        TYPES.forEach(type => {
            newData[type].total = newData[type].dataList.reduce((sum, x) => sum + x.bet, 0);
        });

        this.setState({
            data: newData
        });
    }

    handleStop(message) {
        const newData = this.state.data;
        const result = message.finish.result;
        newData.roundNumber = message.finish.roundId;
        newData.result = result;
        newData.settlementResult = this.getSettlementResult(message.finish.settlementResult);
        newData.resultSumText = result.reduce((sum, x) => sum + x, 0)
        newData.time.remainingTime = WAITING_TIME;

        this.setState({
            data: newData,
            gameState: GameState.waiting,
        });

        this.countDownToStart.call(this, newData, WAITING_TIME + 1);
    }

    countDownToStart(newData, time) {
        setTimeout(() => {
            newData.time.remainingTime = TOTAL_TIME;
            newData.roundNumber += 1;
            this.setState({
                data: newData,
                gameState: GameState.running,
            })
        }, time * 1000);
    }

    getSettlementResult(result) {
        const resultArr = [];
        TYPES.forEach(function (type) {
            if (result[type]) {
                resultArr.push(type.toUpperCase());
            }
        })

        return resultArr.join(" - ");
    }

    getBet(type, userObj) {
        const betItem = new BetItem();
        betItem.name = userObj.name;
        betItem.bet = userObj[type];

        return betItem;
    }

    showUpdater() {
        this.setState({
            isShowResultUpdater: true
        })
    }
}