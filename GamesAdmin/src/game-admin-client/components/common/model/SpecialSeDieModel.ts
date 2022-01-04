import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import TimeModel from "./TimeModel";

export class SpecialSeDieBetList {
  even: BetList;
  odd: BetList;

  constructor() {
    this.even = new BetList();
    this.odd = new BetList();
  }
}

export default class SpecialSeDieModel extends BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  betList: SpecialSeDieBetList;
  result: Array<DiceModel>;
  gameState: number;
  isOpenControlResultModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoundResult: { [key: string]: boolean };

  constructor() {
    super();

    this.result = [
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
      new DiceModel(-1, DiceType.XocXoc),
    ];
    this.betList = new SpecialSeDieBetList();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenControlResultModal = false;
    this.currentDiceNumber = -1;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = {};
  }
}
