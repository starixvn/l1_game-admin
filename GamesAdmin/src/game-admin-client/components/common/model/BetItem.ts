export default class BetItem {
    name: string
    bet: number
    time: string
    choice: string

    constructor(name: string = '', bet: number = 0, time: string = '', choice = '') {
        this.name = name;
        this.bet = bet;
        this.time = time;
        this.choice = choice;
    }
}