import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class KenoPowerBetListModel {
  blue: BetList;
  red: BetList;
  big: BetList;
  small: BetList;
  odd: BetList;
  even: BetList;
  blueBig: BetList;
  blueSmall: BetList;
  redBig: BetList;
  redSmall: BetList;

  constructor() {
    this.blue = new BetList();
    this.red = new BetList();
    this.big = new BetList();
    this.small = new BetList();
    this.odd = new BetList();
    this.even = new BetList();
    this.blueBig = new BetList();
    this.blueSmall = new BetList();
    this.redBig = new BetList();
    this.redSmall = new BetList();
  }
}

class KenoPowerSettlementResultModel {
  blue: boolean;
  red: boolean;
  big: boolean;
  small: boolean;
  odd: boolean;
  even: boolean;
  blueBig: boolean;
  blueSmall: boolean;
  redBig: boolean;
  redSmall: boolean;
}

class KenoPowerResult {
  blue: Array<number>;
  red: Array<number>;

  constructor() {
    this.blue = [];
    this.red = [];
  }
}

export class KenoPowerModel extends BaseMonitoringState {
  roundNumber: number;
  betList: KenoPowerBetListModel;
  time: TimeModel;
  result: KenoPowerResult;
  settlementResult: KenoPowerSettlementResultModel;
  gameState: number;
  isOpenModal: boolean;
  currentSelectedIndex: number;
  constructor() {
    super();
    this.result = new KenoPowerResult();
    this.settlementResult = new KenoPowerSettlementResultModel();
    this.betList = new KenoPowerBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenModal = false;
  }
}
