import BetList from "./BetList";
import TimeModel from "./TimeModel";
import CardResult from "./CardResult";
import { GameState } from "../../shared/common/GameState";
import BaseMonitoringState from "./BaseMonitoringModel";

class BaccaratBetList {
  player: BetList;
  banker: BetList;
  tie: BetList;
  playerPair: BetList;
  bankerPair: BetList;
  big: BetList;
  small: BetList;

  constructor() {
    this.player = new BetList();
    this.banker = new BetList();
    this.tie = new BetList();
    this.playerPair = new BetList();
    this.bankerPair = new BetList();
    this.big = new BetList();
    this.small = new BetList();
  }
}

class BaccaratModel extends BaseMonitoringState {
  result: { bankerCards: CardResult[]; playerCards: CardResult[] };
  isShowResultUpdater: boolean;
  isShowDeskOfCards: boolean;
  selectedCard: string;
  cardIndex: number;

  constructor() {
    super();

    this.result = {
      bankerCards: [new CardResult(), new CardResult(), new CardResult()],
      playerCards: [new CardResult(), new CardResult(), new CardResult()],
    };
    this.time = new TimeModel();
    this.gameState = GameState.none;
    this.isShowResultUpdater = false;
    this.isShowDeskOfCards = false;
    this.selectedCard = "";
    this.cardIndex = -1;
  }
}

export { BaccaratBetList, BaccaratModel };
