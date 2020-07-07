import BetList from './BetList'
import TimeModel from './TimeModel'
import CardResult from './CardResult'
import { GameState } from '../../shared/common/GameState'

class BaccaratBetList {
    player: BetList;
    banker: BetList;
    tie: BetList;
    playerPair: BetList;
    bankerPair: BetList;
    big: BetList;
    small: BetList;

    constructor() {
        this.player = new BetList();
        this.banker = new BetList();
        this.tie = new BetList();
        this.playerPair = new BetList();
        this.bankerPair = new BetList();
        this.big = new BetList();
        this.small = new BetList();
    }
}

export default class BaccaratModel {
    roundNumber: number;
    time: TimeModel;
    betList: BaccaratBetList;
    result: { bankerCards: CardResult[], playerCards: CardResult[] };
    settlementResult: {};
    gameState: number;
    isShowResultUpdater: boolean;
    isShowDeskOfCards: boolean;
    selectedCard: string;
    cardIndex: number;

    constructor() {
        this.result = { bankerCards: [new CardResult(), new CardResult(), new CardResult()], playerCards: [new CardResult(), new CardResult(), new CardResult()] };
        this.betList = new BaccaratBetList();
        this.time = new TimeModel();
        this.gameState = GameState.none;
        this.isShowResultUpdater = false;
        this.isShowDeskOfCards = false;
        this.selectedCard = "";
        this.cardIndex = -1;
    }
}