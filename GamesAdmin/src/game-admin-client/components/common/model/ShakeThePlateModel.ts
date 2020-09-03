import BetList from "./BetList";
import DiceModel from "./DiceModel";
import TimeModel from "./TimeModel";

class ShakeThePlaceBetList {
  odd: BetList;
  even: BetList;
  red3: BetList;
  red4: BetList;
  white3: BetList;
  white4: BetList;

  constructor() {
    this.odd = new BetList();
    this.even = new BetList();
    this.red3 = new BetList();
    this.red4 = new BetList();
    this.white3 = new BetList();
    this.white4 = new BetList();
  }
}

export default class ShakeThePlaceModel {
  roundNumber: number;
  time: TimeModel;
  betList: ShakeThePlaceBetList;
  result: Array<DiceModel>;
  gameState: number;
  disabled: boolean;

  constructor() {
    this.result = [new DiceModel(), new DiceModel(), new DiceModel(), new DiceModel()];
    this.betList = new ShakeThePlaceBetList();
    this.time = new TimeModel();
    this.gameState = 0;
    this.disabled = false;
  }
}

export { ShakeThePlaceModel, ShakeThePlaceBetList }
