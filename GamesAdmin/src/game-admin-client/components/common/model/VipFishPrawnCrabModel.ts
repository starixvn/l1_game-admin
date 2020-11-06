import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";

class VipFishPrawnCrabBetList {
  gourd: BetList;
  fish: BetList;
  crab: BetList;
  rooster: BetList;
  stag: BetList;
  prawn: BetList;
  anyTriple: BetList;

  constructor() {
    this.gourd = new BetList();
    this.fish = new BetList();
    this.crab = new BetList();
    this.rooster = new BetList();
    this.stag = new BetList();
    this.prawn = new BetList();
    this.anyTriple = new BetList();
  }
}

class GameResult {
  "dice-0": number;
  "dice-1": number;
  "dice-2": number;

  constructor(dice0 = -1, dice1 = -1, dice2 = -1) {
    this["dice-0"] = dice0;
    this["dice-1"] = dice1;
    this["dice-2"] = dice2;
  }
}

export default class VipFishPrawnCrabModel {
  roundNumber: number;
  time: TimeModel;
  betList: VipFishPrawnCrabBetList;
  result: Array<DiceModel>;
  isOpenModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoudResult: GameResult;
  winners: Array<boolean>;
  gameState: number;

  constructor() {
    this.result = [
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
    ];
    this.betList = new VipFishPrawnCrabBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.nextRoudResult = new GameResult();
    this.winners = [false, false, false, false, false, false, false];
    this.gameState = 0;
  }
}

export { VipFishPrawnCrabModel, VipFishPrawnCrabBetList, GameResult };
