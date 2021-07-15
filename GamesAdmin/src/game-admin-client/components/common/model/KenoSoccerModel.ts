import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class KenoSoccerBetListModel {
  preHTHome: BetList;
  preHTAway: BetList;
  preHTOver: BetList;
  preHTUnder: BetList;
  preHTOdd: BetList;
  preHTEven: BetList;
  preFTHome: BetList;
  preFTAway: BetList;
  preFTOver: BetList;
  preFTUnder: BetList;
  preFTOdd: BetList;
  preFTEven: BetList;
  liveFTHome: BetList;
  liveFTAway: BetList;
  liveFTOver: BetList;
  liveFTUnder: BetList;
  liveFTOdd: BetList;
  liveFTEven: BetList;
  constructor() {
    this.preHTHome = new BetList();
    this.preHTAway = new BetList();
    this.preHTOver = new BetList();
    this.preHTUnder = new BetList();
    this.preHTOdd = new BetList();
    this.preHTEven = new BetList();
    this.preFTHome = new BetList();
    this.preFTAway = new BetList();
    this.preFTOver = new BetList();
    this.preFTUnder = new BetList();
    this.preFTOdd = new BetList();
    this.preFTEven = new BetList();
    this.liveFTHome = new BetList();
    this.liveFTAway = new BetList();
    this.liveFTOver = new BetList();
    this.liveFTUnder = new BetList();
    this.liveFTOdd = new BetList();
    this.liveFTEven = new BetList();
  }
}

class KenoSoccerPeriodSettlementResultModel {
  home: boolean;
  away: boolean;
  over: boolean;
  under: boolean;
  odd: boolean;
  even: boolean;
}

class KenoSoccerPeriodResult {
  home: Array<number>;
  away: Array<number>;

  constructor(){
      this.home = [];
      this.away = [];
  }
}

class KenoSoccerPeriod {
  result: KenoSoccerPeriodResult;
  settlementResult: KenoSoccerPeriodSettlementResultModel;
  sum: number;

  constructor() {
    this.settlementResult = new KenoSoccerPeriodSettlementResultModel();
    this.result = new KenoSoccerPeriodResult();
  }
}

class KenoSoccerResult {
  halfTime: KenoSoccerPeriod;
  fullTime: KenoSoccerPeriod;

  constructor() {
    this.halfTime = new KenoSoccerPeriod();
    this.fullTime = new KenoSoccerPeriod();
  }
}

export class KenoSoccerModel {
  roundNumber: number;
  betList: KenoSoccerBetListModel;
  time: TimeModel;
  result: KenoSoccerResult;
  gameState: number;
  isOpenModal: boolean;
  currentSelectedIndex: number;
  constructor() {
    this.result = new KenoSoccerResult();
    this.betList = new KenoSoccerBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.isOpenModal = false;
  }
}
