import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";

class SeDieBetList {
  odd: BetList;
  even: BetList;
  red2: BetList;
  red3: BetList;
  red4: BetList;
  white3: BetList;
  white4: BetList;

  constructor() {
    this.odd = new BetList();
    this.even = new BetList();
    this.red2 = new BetList();
    this.red3 = new BetList();
    this.red4 = new BetList();
    this.white3 = new BetList();
    this.white4 = new BetList();
  }
}

class SeDieSettlementResult {
  odd: boolean;
  even: boolean;
  red2: boolean;
  red3: boolean;
  red4: boolean;
  white3: boolean;
  white4: boolean;
}

export default class ShakeThePlateModel extends BaseMonitoringState {
  result: Array<DiceModel>;
  settlementResult: SeDieSettlementResult;
  currentDiceNumber: number;
  nextRoundResult: { [key: string]: boolean };

  constructor() {
    super();
    this.currentDiceNumber = -1;
    this.nextRoundResult = {};
    this.betList = new SeDieBetList();
    this.settlementResult = new SeDieSettlementResult();
    this.result = [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
  }
}

export {
  ShakeThePlateModel,
  SeDieBetList as ShakeThePlateBetList,
  SeDieSettlementResult,
};
