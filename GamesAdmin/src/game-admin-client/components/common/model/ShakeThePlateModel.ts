import BetList from "./BetList";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import TimeModel from "./TimeModel";

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

export default class ShakeThePlateModel {
  roundNumber: number;
  time: TimeModel;
  betList: SeDieBetList;
  result: Array<DiceModel>;
  gameState: number;
  disabled: boolean;
  settlementResult: SeDieSettlementResult;
  isOpenControlResultModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoundResult: { [key: string]: boolean };

  constructor() {
    this.result = [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
    this.betList = new SeDieBetList();
    this.time = new TimeModel();
    this.gameState = 0;
    this.disabled = false;
    this.settlementResult = new SeDieSettlementResult();
    this.isOpenControlResultModal = false;
    this.currentDiceNumber = -1;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = {};
  }
}

export { ShakeThePlateModel, SeDieBetList as ShakeThePlateBetList };
