import TimeModel from "./TimeModel";
import { GameState } from "../../shared/common/GameState";
import BetList from "./BetList";

class RouletteBetList {
    straightUp: BetList;
    split: BetList;
    street: BetList;
    triangle: BetList;
    corner: BetList;
    fourNumbers: BetList;
    line: BetList;
    column: BetList;
    dozen: BetList;
    redBlack: BetList;
    oddEven: BetList;
    bigSmall: BetList;

    constructor() {
        this.straightUp = new BetList();
        this.split = new BetList();
        this.street = new BetList();
        this.triangle = new BetList();
        this.corner = new BetList();
        this.fourNumbers = new BetList();
        this.line = new BetList();
        this.column = new BetList();
        this.dozen = new BetList();
        this.redBlack = new BetList();
        this.oddEven = new BetList();
        this.bigSmall = new BetList();
    }
}

export default class RouletteModel {
    roundNumber: number;
    time: TimeModel;
    result: number;
    gameState: number;
    betList: RouletteBetList;
    isShowResultUpdater: boolean;

    constructor() {
        this.result = -1;
        this.time = new TimeModel();
        this.gameState = GameState.none;
        this.betList = new RouletteBetList();
        this.isShowResultUpdater = false;
    }
}