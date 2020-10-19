import BetList from "./BetList";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import TimeModel from "./TimeModel";

class SpecialSeDieBetList {
  even: BetList;
  odd: BetList;

  constructor() {
    this.even = new BetList();
    this.odd = new BetList();
  }
}

export default class SpecialSeDieModel {
  roundNumber: number;
  time: TimeModel;
  betList: SpecialSeDieBetList;
  result: Array<DiceModel>;
  gameState: number;

  constructor() {
    this.result = [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
    this.betList = new SpecialSeDieBetList();
    this.time = new TimeModel();
    this.gameState = 0;
  }
}
