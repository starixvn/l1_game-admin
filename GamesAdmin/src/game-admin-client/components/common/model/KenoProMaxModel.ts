import BetList from './BetList'
import TimeModel from './TimeModel'

export default class KenoProMaxModel {
    roundNumber: number
    gold: BetList
    wood: BetList
    water: BetList
    fire: BetList
    earth: BetList
    big: BetList
    small: BetList
    even: BetList
    odd: BetList
    time: TimeModel
    result: Array<number>
    resultSumText: string
    settlementResult: string

    constructor() {
        this.result = new Array();
        this.gold = new BetList();
        this.wood = new BetList();
        this.water = new BetList();
        this.fire = new BetList();
        this.earth = new BetList();
        this.big = new BetList();
        this.small = new BetList();
        this.even = new BetList();
        this.odd = new BetList();
        this.time = new TimeModel();
    }
}