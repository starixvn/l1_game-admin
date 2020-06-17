import BetList from './BetList'
import TimeModel from './TimeModel'
import DiceModel from './DiceModel'

class FishPrawnCrabBetList {
  redBau: BetList;
  redCa: BetList;
  redCua: BetList;
  redGa: BetList;
  redHuou: BetList;
  redTom: BetList;

  constructor() {
    this.redBau = new BetList();
    this.redCa = new BetList();
    this.redCua = new BetList();
    this.redGa = new BetList();
    this.redHuou = new BetList();
    this.redTom = new BetList();
  }
}

class GameResult {
  'dice-0': number;
  'dice-1': number;
  'dice-2': number;

  constructor(dice0 = -1, dice1 = -1, dice2 = -1) {
    this["dice-0"] = dice0;
    this["dice-1"] = dice1;
    this["dice-2"] = dice2;
  }
}

export default class FishPrawnCrabModel {
  roundNumber: number
  time: TimeModel
  betList: FishPrawnCrabBetList
  result: Array<DiceModel>
  isOpenModal: boolean
  currentDiceNumber: number
  isHandleNextRoundResult: boolean
  nextRoudResult: GameResult
  winers: Array<boolean>
  gameState: number

  constructor() {
    this.result = new Array(new DiceModel(), new DiceModel(), new DiceModel(), new DiceModel());
    this.betList = new FishPrawnCrabBetList();
    this.time = new TimeModel();
    this.isOpenModal = false;
    this.isHandleNextRoundResult = false;
    this.nextRoudResult = new GameResult();
    this.winers = new Array(false, false, false, false, false, false)
    this.gameState = 0;
  }
}

export { FishPrawnCrabModel, FishPrawnCrabBetList, GameResult }
