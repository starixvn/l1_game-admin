import BetList from "./BetList";
import TimeModel from "./TimeModel";

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
  constructor(rank, suit) {
    this.rank = rank;
    this.suit = suit;
  }
}

class SettlementResultModel {
  red: boolean;
  black: boolean;
  diamonds: boolean;
  hearts: boolean;
  spades: boolean;
  clubs: boolean;
}

export class ThreeCardsModel {
  roundNumber: number;
  betList: ThreeCardsBetListModel;
  time: TimeModel;
  result: Array<CardModel>;
  gameState: number;
  settlementResult: SettlementResultModel;
  isOpenModal: boolean;
  currentSelectedIndex: number;
  constructor() {
    this.result = [new CardModel(), new CardModel(), new CardModel()];
    this.betList = new ThreeCardsBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.settlementResult = new SettlementResultModel();
    this.isOpenModal = false;
  }
}
