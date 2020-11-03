import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";

class VipFishPrawnCrabBetList {
  redBau: BetList;
  redCa: BetList;
  redCua: BetList;
  redGa: BetList;
  redHuou: BetList;
  redTom: BetList;
  redAnyTriple: BetList;

  constructor() {
    this.redBau = new BetList();
    this.redCa = new BetList();
    this.redCua = new BetList();
    this.redGa = new BetList();
    this.redHuou = new BetList();
    this.redTom = new BetList();
    this.redAnyTriple = new BetList();
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
    this.winners = [false, false, false, false, false, false];
    this.gameState = 0;
  }
}

export { VipFishPrawnCrabModel, VipFishPrawnCrabBetList, GameResult };
