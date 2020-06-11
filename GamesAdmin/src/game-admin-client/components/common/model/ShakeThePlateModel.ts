import BetList from './BetList'
import TimeModel from './TimeModel'
import DiceModel from './DiceModel'

class ShakeThePlaceBetList {
    chan: BetList;
    le: BetList;
    red3: BetList;
    red4: BetList;
    white3: BetList;
    white4: BetList;

    constructor() {
        this.chan = new BetList();
        this.le = new BetList();
        this.red3 = new BetList();
        this.red4 = new BetList();
        this.white3 = new BetList();
        this.white4 = new BetList();
    }
}

export default class ShakeThePlaceModel {
    roundNumber: number
    time: TimeModel
    betList: ShakeThePlaceBetList
    result: Array<DiceModel>

    constructor() {
        this.result = new Array(new DiceModel(), new DiceModel(), new DiceModel(), new DiceModel());
        this.betList = new ShakeThePlaceBetList();
        this.time = new TimeModel();
    }
}

export { ShakeThePlaceModel, ShakeThePlaceBetList }