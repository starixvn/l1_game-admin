import BetList from "./BetList";
import TimeModel from "./TimeModel";

export class BitcoinMasterBetListModel {
  up: BetList;
  down: BetList;
  odd: BetList;
  even: BetList;

  constructor() {
    this.up = new BetList();
    this.down = new BetList();
    this.odd = new BetList();
    this.even = new BetList();
  }
}

export class BitcoinMasterResultModel {
  open: number;
  close: number;
}

class SettlementResultModel {
  up: boolean;
  down: boolean;
  odd: boolean;
  even: boolean;
}

export class BitcoinMasterModel {
  roundNumber: number;
  betList: BitcoinMasterBetListModel;
  time: TimeModel;
  result: BitcoinMasterResultModel;
  gameState: number;
  settlementResult: SettlementResultModel;

  constructor() {
    this.result = {
      open: 0,
      close: 0,
    };

    this.betList = new BitcoinMasterBetListModel();
    this.time = new TimeModel();
    this.gameState = 0;
    this.settlementResult = new SettlementResultModel();
  }
}
