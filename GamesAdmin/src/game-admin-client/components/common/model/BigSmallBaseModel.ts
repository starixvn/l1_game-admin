import BetList from "./BetList";
import BaseMonitoringState from "./BaseMonitoringModel";

class BigSmallBaseBetList {
  firstBetType: BetList;
  secondBetType: BetList;

  constructor() {
    this.firstBetType = new BetList();
    this.secondBetType = new BetList();
  }
}

export default class BigSmallBaseModel extends BaseMonitoringState {
  firstBetTypeUsers: number;
  secondBetTypeUsers: number;
  isHavingResult: boolean;
  currentState: number;
  isOpenControlResultModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoundResult: { [key: string]: number };

  constructor() {
    super();

    this.isHavingResult = false;
    this.isOpenControlResultModal = false;
    this.currentDiceNumber = -1;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = {
      dice1: -1,
      dice2: -1,
      dice3: -1,
    };
  }
}

export { BigSmallBaseModel, BigSmallBaseBetList };
