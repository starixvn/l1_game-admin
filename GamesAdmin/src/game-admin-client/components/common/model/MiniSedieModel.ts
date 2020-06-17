import BetList from './BetList'
import TimeModel from './TimeModel'
import DiceModel from './DiceModel'

class MiniSedieBetList {
    even: BetList;
    odd: BetList;
    red3: BetList;
    red4: BetList;
    white3: BetList;
    white4: BetList;

    constructor() {
        this.even = new BetList();
        this.odd = new BetList();
        this.red3 = new BetList();
        this.red4 = new BetList();
        this.white3 = new BetList();
        this.white4 = new BetList();
    }
}

export default class MiniSedieModel {
    roundNumber: number
    time: TimeModel
    betList: MiniSedieBetList
    result: Array<DiceModel>
    gameState: number

    constructor() {
        this.result = new Array(new DiceModel(), new DiceModel(), new DiceModel(), new DiceModel());
        this.betList = new MiniSedieBetList();
        this.time = new TimeModel();
        this.gameState = 0;
    }
}

export { MiniSedieModel as ShakeThePlaceModel, MiniSedieBetList }
