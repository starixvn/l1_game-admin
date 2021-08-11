import BetList from "./BetList";
import TimeModel from "./TimeModel";
import CardResult from "./CardResult";

class DragonTigerBetList {
  dragon: BetList;
  tie: BetList;
  tiger: BetList;

  constructor() {
    this.dragon = new BetList();
    this.tiger = new BetList();
    this.tie = new BetList();
  }
}

export default class DragonTigerModel {
  roundNumber: number;
  betList: DragonTigerBetList;
  time: TimeModel;
  result: Array<CardResult>;

  constructor() {
    this.result = [new CardResult(), new CardResult()];
    this.betList = new DragonTigerBetList();
    this.time = new TimeModel();
  }
}
