import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class KenoPowerBetListModel {
  player: BetList;
  banker: BetList;
  big: BetList;
  small: BetList;
  odd: BetList;
  even: BetList;
  red: BetList;
  orange: BetList;
  yellow: BetList;
  green: BetList;
  blue: BetList;
  indigo: BetList;
  violet: BetList;
  playerBig: BetList;
  playerSmall: BetList;
  bankerBig: BetList;
  bankerSmall: BetList;

  constructor() {
    this.player = new BetList();
    this.banker = new BetList();
    this.big = new BetList();
    this.small = new BetList();
    this.odd = new BetList();
    this.even = new BetList();
    this.red = new BetList();
    this.orange = new BetList();
    this.yellow = new BetList();
    this.green = new BetList();
    this.blue = new BetList();
    this.indigo = new BetList();
    this.violet = new BetList();
    this.playerBig = new BetList();
    this.playerSmall = new BetList();
    this.bankerBig = new BetList();
    this.bankerSmall = new BetList();
  }
}

class KenoPowerSettlementResultModel {
  player: boolean;
  banker: boolean;
  big: boolean;
  small: boolean;
  odd: boolean;
  even: boolean;
  red: boolean;
  orange: boolean;
  yellow: boolean;
  green: boolean;
  blue: boolean;
  indigo: boolean;
  violet: boolean;
  playerBig: boolean;
  playerSmall: boolean;
  bankerBig: boolean;
  bankerSmall: boolean;
}

class KenoPowerResult {
  player: Array<number>;
  banker: Array<number>;

  constructor() {
    this.player = [];
    this.banker = [];
  }
}

export class KenoPowerModel {
  roundNumber: number;
  betList: KenoPowerBetListModel;
  time: TimeModel;
  result: KenoPowerResult;
  settlementResult: KenoPowerSettlementResultModel;
  gameState: number;
  isOpenModal: boolean;
  currentSelectedIndex: number;
  constructor() {
    this.result = new KenoPowerResult();
    this.settlementResult = new KenoPowerSettlementResultModel();
    this.betList = new KenoPowerBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenModal = false;
  }
}
