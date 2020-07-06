import BetList from './BetList'
import TimeModel from './TimeModel'
import DiceModel from './DiceModel'

class BigSmallBaseBetList {
  firstBetType: BetList;
  secondBetType: BetList;

  constructor() {
    this.firstBetType = new BetList();
    this.secondBetType = new BetList();
  }
}

export default class BigSmallBaseModel {
  roundNumber: number;
  time: TimeModel;
  betList: BigSmallBaseBetList;
  result: Array<DiceModel>;
  type: string;
  turbo: string;
  firstBetTypeUsers: number;
  secondBetTypeUsers: number;
  isHavingResult: boolean;
  currentState: number;

  constructor() {
    this.result = [new DiceModel(), new DiceModel(), new DiceModel()];
    this.betList = new BigSmallBaseBetList();
    this.time = new TimeModel();
    this.isHavingResult = false;
    this.currentState = 0;
  }
}

export { BigSmallBaseModel, BigSmallBaseBetList }
