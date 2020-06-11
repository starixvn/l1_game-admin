export default class BetItem {
    name: string
    bet: number
    time: string

    constructor(name: string = '', bet: number = 0, time: string = '') {
        this.name = name;
        this.bet = bet;
        this.time = time
    }
}