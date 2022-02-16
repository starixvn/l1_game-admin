import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import BaseMonitoringState from "./BaseMonitoringModel";

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

export default class MiniSedieModel extends BaseMonitoringState {
  betList: MiniSedieBetList;
  result: Array<DiceModel>;
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
    this.betList = new MiniSedieBetList();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenControlResultModal = false;
    this.currentDiceNumber = -1;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = {};
  }
}

export { MiniSedieModel as ShakeThePlateModel, MiniSedieBetList };
