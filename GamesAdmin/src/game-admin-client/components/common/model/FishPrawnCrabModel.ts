import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import BaseMonitoringState from "./BaseMonitoringModel";

class FishPrawnCrabBetList {
  gourd: BetList;
  fish: BetList;
  crab: BetList;
  rooster: BetList;
  stag: BetList;
  prawn: BetList;

  constructor() {
    this.fish = new BetList();
    this.gourd = new BetList();
    this.rooster = new BetList();
    this.crab = new BetList();
    this.stag = new BetList();
    this.prawn = new BetList();
  }
}

class GameResult {
  "dice1": number;
  "dice2": number;
  "dice3": number;

  constructor(dice0 = 0, dice1 = 0, dice2 = 0) {
    this["dice1"] = dice0;
    this["dice2"] = dice1;
    this["dice3"] = dice2;
  }
}

export default class FishPrawnCrabModel extends BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  result: Array<DiceModel>;
  isOpenModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoundResult: GameResult;
  settlementResult: Array<boolean>;
  gameState: number;

  constructor() {
    super();

    this.result = [
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
    ];
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = new GameResult();
    this.settlementResult = [false, false, false, false, false, false];
    this.gameState = 0;
  }
}

export { FishPrawnCrabModel, FishPrawnCrabBetList, GameResult };
