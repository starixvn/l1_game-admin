import BetList from "./BetList";
import TimeModel from "./TimeModel";
import DiceModel from "./DiceModel";
import DiceType from "./DiceType";

class VipSeDieWarSettlementResult {
  even: boolean;
  odd: boolean;
  tie: boolean;
  player: boolean;
  banker: boolean;

  playerOdd: boolean;
  playerEven: boolean;
  playerRed3: boolean;
  playerRed4: boolean;
  playerWhite3: boolean;
  playerWhite4: boolean;

  bankerOdd: boolean;
  bankerEven: boolean;
  bankerRed3: boolean;
  bankerRed4: boolean;
  bankerWhite3: boolean;
  bankerWhite4: boolean;
}

class VipSeDieWarBetList {
  even: BetList;
  odd: BetList;
  tie: BetList;
  player: BetList;
  banker: BetList;

  playerOdd: BetList;
  playerEven: BetList;
  playerRed3: BetList;
  playerRed4: BetList;
  playerWhite3: BetList;
  playerWhite4: BetList;

  bankerOdd: BetList;
  bankerEven: BetList;
  bankerRed3: BetList;
  bankerRed4: BetList;
  bankerWhite3: BetList;
  bankerWhite4: BetList;

  constructor() {
    this.even = new BetList();
    this.odd = new BetList();
  }
}

class VipSeDieWarResult {
  banker: Array<DiceModel>;
  player: Array<DiceModel>;
}

export default class VipSeDieWarModel {
  roundNumber: number;
  time: TimeModel;
  betList: VipSeDieWarBetList;
  result: VipSeDieWarResult;
  gameState: number;
  settlementResult: VipSeDieWarSettlementResult;

  constructor() {
    this.result = {
      banker: [
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
      ],
      player: [
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
        new DiceModel(-1, DiceType.XocXoc),
      ],
    };

    this.betList = new VipSeDieWarBetList();
    this.time = new TimeModel();
    this.gameState = 0;
  }
}

export { VipSeDieWarModel, VipSeDieWarBetList };
