import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";

class VipSicBoBetList {
  "1point": BetList;
  "2points": BetList;
  "3points": BetList;
  "4points": BetList;
  "5points": BetList;
  "6points": BetList;
  big: BetList;
  small: BetList;
  odd: BetList;
  even: BetList;

  constructor() {
    this["1point"] = new BetList();
    this["2points"] = new BetList();
    this["3points"] = new BetList();
    this["4points"] = new BetList();
    this["5points"] = new BetList();
    this["6points"] = new BetList();
    this.big = new BetList();
    this.small = new BetList();
    this.odd = new BetList();
    this.even = new BetList();
  }
}

class VipSicBoSettlementResult {
  "1point": boolean;
  "2points": boolean;
  "3points": boolean;
  "4points": boolean;
  "5points": boolean;
  "6points": boolean;
  big: boolean;
  small: boolean;
  odd: boolean;
  even: boolean;
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

export default class VipSicBoModel {
  roundNumber: number;
  time: TimeModel;
  betList: VipSicBoBetList;
  result: DiceModel[];
  isOpenModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoudResult: DiceModel[];
  gameState: number;
  settlementResult: VipSicBoSettlementResult;

  constructor() {
    this.result = [
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
      new DiceModel(-1, DiceType.TaiXiu),
    ];
    this.betList = new VipSicBoBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.gameState = 0;
    this.nextRoudResult = [];
    this.settlementResult = new VipSicBoSettlementResult();
  }
}

export { VipSicBoModel, VipSicBoBetList, GameResult };
