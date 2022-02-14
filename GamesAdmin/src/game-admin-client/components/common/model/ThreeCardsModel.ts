import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";

export class ThreeCardsBetListModel {
  red: BetList;
  black: BetList;
  diamonds: BetList;
  hearts: BetList;
  spades: BetList;
  clubs: BetList;

  constructor() {
    this.red = new BetList();
    this.black = new BetList();
    this.diamonds = new BetList();
    this.hearts = new BetList();
    this.spades = new BetList();
    this.clubs = new BetList();
  }
}

export class CardModel {
  rank: string;
  suit: number;
}

export class ThreeCardsSettlementResultModel {
  red: boolean;
  black: boolean;
  diamonds: boolean;
  hearts: boolean;
  spades: boolean;
  clubs: boolean;
}

export class ThreeCardsModel extends BaseMonitoringState {
  roundNumber: number;
  betList: ThreeCardsBetListModel;
  result: Array<CardModel>;
  settlementResult: ThreeCardsSettlementResultModel;
  isOpenModal: boolean;
  currentSelectedIndex: number;

  constructor() {
    super();

    this.result = [new CardModel(), new CardModel(), new CardModel()];
    this.betList = new ThreeCardsBetListModel();
    this.settlementResult = new ThreeCardsSettlementResultModel();
    this.isOpenModal = false;
  }
}
