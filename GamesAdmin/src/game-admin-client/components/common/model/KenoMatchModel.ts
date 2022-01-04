import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class KenoMatchBetListModel {
  fhBlue: BetList;
  fhRed: BetList;
  fhOver: BetList;
  fhUnder: BetList;
  fhOdd: BetList;
  fhEven: BetList;
  ftBlue: BetList;
  ftRed: BetList;
  ftOver: BetList;
  ftUnder: BetList;
  ftOdd: BetList;
  ftEven: BetList;
  liveFTBlue: BetList;
  liveFTRed: BetList;
  liveFTOver: BetList;
  liveFTUnder: BetList;
  liveFTOdd: BetList;
  liveFTEven: BetList;
  constructor() {
    this.fhBlue = new BetList();
    this.fhRed = new BetList();
    this.fhOver = new BetList();
    this.fhUnder = new BetList();
    this.fhOdd = new BetList();
    this.fhEven = new BetList();
    this.ftBlue = new BetList();
    this.ftRed = new BetList();
    this.ftOver = new BetList();
    this.ftUnder = new BetList();
    this.ftOdd = new BetList();
    this.ftEven = new BetList();
    this.liveFTBlue = new BetList();
    this.liveFTRed = new BetList();
    this.liveFTOver = new BetList();
    this.liveFTUnder = new BetList();
    this.liveFTOdd = new BetList();
    this.liveFTEven = new BetList();
  }
}

class KenoMatchPeriodSettlementResultModel {
  blue: boolean;
  red: boolean;
  over: boolean;
  under: boolean;
  odd: boolean;
  even: boolean;
}

class KenoMatchPeriodResult {
  blue: Array<number>;
  red: Array<number>;

  constructor() {
    this.blue = [];
    this.red = [];
  }
}

class KenoMatchPeriod {
  result: KenoMatchPeriodResult;
  settlementResult: KenoMatchPeriodSettlementResultModel;
  sum: number;

  constructor() {
    this.settlementResult = new KenoMatchPeriodSettlementResultModel();
    this.result = new KenoMatchPeriodResult();
  }
}

class KenoMatchResult {
  firstHalf: KenoMatchPeriod;
  fullTime: KenoMatchPeriod;

  constructor() {
    this.firstHalf = new KenoMatchPeriod();
    this.fullTime = new KenoMatchPeriod();
  }
}

export class KenoMatchModel extends BaseMonitoringState {
  roundNumber: number;
  betList: KenoMatchBetListModel;
  time: TimeModel;
  result: KenoMatchResult;
  gameState: number;
  isOpenModal: boolean;
  currentSelectedIndex: number;
  constructor() {
    super();
    this.result = new KenoMatchResult();
    this.betList = new KenoMatchBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenModal = false;
  }
}
