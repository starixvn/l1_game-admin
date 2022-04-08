import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import TimeModel from "./TimeModel";

class LotteryBetList {
  "4Sets": BetList;
  "27Sets": BetList;
  special: BetList;
  head: BetList;
  tail: BetList;
  others: BetList;

  constructor() {
    this["4Sets"] = new BetList();
    this["27Sets"] = new BetList();
    this.special = new BetList();
    this.head = new BetList();
    this.tail = new BetList();
    this.others = new BetList();
  }
}

class LotteryGameResult {
  special: Array<string>;
  first: Array<string>;
  second: Array<string>;
  third: Array<string>;
  fourth: Array<string>;
  fifth: Array<string>;
  sixth: Array<string>;
  seventh: Array<string>;

  constructor() {
    this.special = [""];
    this.first = [""];
    this.second = ["", ""];
    this.third = ["", "", "", "", "", ""];
    this.fourth = ["", "", "", ""];
    this.fifth = ["", "", "", "", "", ""];
    this.sixth = ["", "", ""];
    this.seventh = ["", "", "", ""];
  }
}

export default class LotteryModel extends BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  betList: LotteryBetList;
  result: LotteryGameResult;
  isOpenModal: boolean;
  isHandleNextRoundResult: boolean;
  nextRoudResult: LotteryGameResult[];
  gameState: number;

  constructor() {
    super();

    this.result = new LotteryGameResult();
    this.betList = new LotteryBetList();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.nextRoudResult = [];
  }
}

export { LotteryModel, LotteryBetList, LotteryGameResult };
