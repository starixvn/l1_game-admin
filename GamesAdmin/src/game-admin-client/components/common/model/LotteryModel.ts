import BetList from "./BetList";
import TimeModel from "./TimeModel";

class LotteryBetList {
  "Sets4": BetList;
  "Sets27": BetList;
  special: BetList;
  head: BetList;
  tail: BetList;
  others: BetList;

  constructor() {
    this["Sets4"] = new BetList();
    this["Sets27"] = new BetList();
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

export default class LotteryModel {
  roundNumber: number;
  time: TimeModel;
  betList: LotteryBetList;
  result: LotteryGameResult;
  isOpenModal: boolean;
  isHandleNextRoundResult: boolean;
  nextRoudResult: LotteryGameResult[];
  gameState: number;

  constructor() {
    this.result = new LotteryGameResult();
    this.betList = new LotteryBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.gameState = 0;
    this.nextRoudResult = [];
  }
}

export { LotteryModel, LotteryBetList, LotteryGameResult };
