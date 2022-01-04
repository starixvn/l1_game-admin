import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";
import TimeModel from "./TimeModel";

class VipFishPrawnCrabBetList {
  gourd: BetList;
  fish: BetList;
  crab: BetList;
  rooster: BetList;
  stag: BetList;
  prawn: BetList;
  anyTriple: BetList;
  mountain: BetList;
  ocean: BetList;

  constructor() {
    this.gourd = new BetList();
    this.fish = new BetList();
    this.crab = new BetList();
    this.rooster = new BetList();
    this.stag = new BetList();
    this.prawn = new BetList();
    this.anyTriple = new BetList();
    this.mountain = new BetList();
    this.ocean = new BetList();
  }
}

class VipFishPrawnCrabSettlementResult {
  gourd: boolean;
  fish: boolean;
  crab: boolean;
  rooster: boolean;
  stag: boolean;
  prawn: boolean;
  anyTriple: boolean;
  mountain: boolean;
  ocean: boolean;
}

class GameResult {
  "dice1": number;
  "dice2": number;
  "dice3": number;

  constructor(dice0 = -1, dice1 = -1, dice2 = -1) {
    this["dice1"] = dice0;
    this["dice2"] = dice1;
    this["dice3"] = dice2;
  }
}

export default class VipFishPrawnCrabModel extends BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  betList: VipFishPrawnCrabBetList;
  result: Array<DiceModel>;
  isOpenModal: boolean;
  currentDiceNumber: number;
  isHandleNextRoundResult: boolean;
  nextRoundResult: GameResult;
  gameState: number;
  settlementResult: VipFishPrawnCrabSettlementResult;

  constructor() {
    super();
    this.result = [
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
      new DiceModel(-1, DiceType.BauCua),
    ];
    this.betList = new VipFishPrawnCrabBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.nextRoundResult = new GameResult();
    this.gameState = 0;
    this.settlementResult = new VipFishPrawnCrabSettlementResult();
  }
}

export {
  VipFishPrawnCrabModel,
  VipFishPrawnCrabBetList,
  GameResult,
  VipFishPrawnCrabSettlementResult,
};
