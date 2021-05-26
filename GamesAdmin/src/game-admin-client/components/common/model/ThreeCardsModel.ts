import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class ThreeCardsBetList {
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

export class Card {
    rank: string;
    suit: number;
}

class SettlementResult {
    red: boolean;
    black: boolean;
    diamonds: boolean;
    hearts: boolean;
    spades: boolean;
    clubs: boolean;
}

export class ThreeCardsModel {
  roundNumber: number;
  betList: ThreeCardsBetList;
  time: TimeModel;
  result: Array<Card>;
  gameState: number;
  settlementResult: SettlementResult;

  constructor() {
    this.result = [new Card(), new Card(), new Card()];
    this.betList = new ThreeCardsBetList();
    this.time = new TimeModel();
    this.gameState = 0;
    this.settlementResult = new SettlementResult();
  }
}
