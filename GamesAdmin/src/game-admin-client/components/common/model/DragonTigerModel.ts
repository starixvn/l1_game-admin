import BetList from './BetList'
import TimeModel from './TimeModel'
import CardResult from './CardResult'

export default class DragonTigerModel {
    roundNumber: number
    dragon: BetList
    tie: BetList
    tiger: BetList
    time: TimeModel
    result: Array<CardResult>

    constructor() {
        this.result = new Array(new CardResult(), new CardResult());
        this.dragon = new BetList();
        this.tiger = new BetList();
        this.tie = new BetList();

        this.time = new TimeModel();
    }
}