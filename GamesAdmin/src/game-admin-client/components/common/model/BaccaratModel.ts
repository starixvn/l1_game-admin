import BetList from './BetList'
import TimeModel from './TimeModel'
import CardResult from './CardResult'

export default class BaccaratModel {
    roundNumber: number
    player: BetList
    banker: BetList
    tie: BetList
    playerPair: BetList
    bankerPair: BetList
    big: BetList
    small: BetList
    result: { bankerCards: CardResult, playerCards: CardResult }
    resultSumText: string
    settlementResult: string
    time: TimeModel

    constructor() {
        this.result = { bankerCards: new CardResult(), playerCards: new CardResult() };
        this.player = new BetList();
        this.banker = new BetList();
        this.tie = new BetList();
        this.playerPair = new BetList();
        this.bankerPair = new BetList();
        this.big = new BetList();
        this.time = new TimeModel();
    }
}