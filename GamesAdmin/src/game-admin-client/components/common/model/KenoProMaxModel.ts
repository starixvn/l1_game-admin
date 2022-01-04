import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class KenoProMaxBetList {
  gold: BetList;
  wood: BetList;
  water: BetList;
  fire: BetList;
  earth: BetList;
  big: BetList;
  small: BetList;
  even: BetList;
  odd: BetList;

  constructor() {
    this.gold = new BetList();
    this.wood = new BetList();
    this.water = new BetList();
    this.fire = new BetList();
    this.earth = new BetList();
    this.big = new BetList();
    this.small = new BetList();
    this.even = new BetList();
    this.odd = new BetList();
  }
}

export default class KenoProMaxModel extends BaseMonitoringState {
  roundNumber: number;
  betList: KenoProMaxBetList;
  time: TimeModel;
  result: Array<number>;
  resultSumText: string;
  settlementResult: string;
  gameState: number;
  isShowResultUpdater: boolean;

  constructor() {
    super();
    this.result = [];

    this.time = new TimeModel();
    this.betList = new KenoProMaxBetList();
  }
}
