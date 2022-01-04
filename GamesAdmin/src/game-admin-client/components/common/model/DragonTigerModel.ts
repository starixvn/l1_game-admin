import BetList from "./BetList";
import BaseMonitoringState from "./BaseMonitoringModel";

export class DragonTigerBetList {
  dragon: BetList;
  tie: BetList;
  tiger: BetList;

  constructor() {
    this.dragon = new BetList();
    this.tiger = new BetList();
    this.tie = new BetList();
  }
}

export default class DragonTigerModel extends BaseMonitoringState {
  isShowDeskOfCards: boolean;
  selectedCard: string;

  constructor() {
    super();
    this.isShowDeskOfCards = false;
    this.selectedCard = "";
  }
}
