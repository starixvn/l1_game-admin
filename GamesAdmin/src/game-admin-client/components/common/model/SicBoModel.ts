import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import BaseMonitoringState from "./BaseMonitoringModel";

class SicBoBetList {
  specificNumber: BetList;
  big: BetList;
  small: BetList;
  anyTriple: BetList;
  totalPoints: BetList;

  constructor() {
    this.specificNumber = new BetList();
    this.big = new BetList();
    this.small = new BetList();
    this.anyTriple = new BetList();
    this.totalPoints = new BetList();
  }
}

class SicBoSettlementResult {
  specific1: boolean;
  specific2: boolean;
  specific3: boolean;
  specific4: boolean;
  specific5: boolean;
  specific6: boolean;
  big: boolean;
  small: boolean;
  anyTriple: boolean;
  total4: boolean;
  total5: boolean;
  total6: boolean;
  total7: boolean;
  total8: boolean;
  total9: boolean;
  total10: boolean;
  total11: boolean;
  total12: boolean;
  total13: boolean;
  total14: boolean;
  total15: boolean;
  total16: boolean;
  total17: boolean;
}

class GameResult {
  "dice-0": number;
  "dice-1": number;
  "dice-2": number;

  constructor(dice0 = -1, dice1 = -1, dice2 = -1) {
    this["dice-0"] = dice0;
    this["dice-1"] = dice1;
    this["dice-2"] = dice2;
  }
}

export default class SicBoModel extends BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  betList: SicBoBetList;
  result: DiceModel[];
  isOpenModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoudResult: DiceModel[];
  gameState: number;
  settlementResult: SicBoSettlementResult;

  constructor() {
    super();
    this.result = [
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
    ];
    this.betList = new SicBoBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.gameState = 0;
    this.nextRoudResult = [];
    this.settlementResult = new SicBoSettlementResult();
  }
}

export { SicBoModel, SicBoBetList, GameResult, SicBoSettlementResult };
