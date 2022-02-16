import BaseMonitoringState from "./BaseMonitoringModel";
import BetList from "./BetList";

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

export class SettlementResultModel {
  up: boolean;
  down: boolean;
  odd: boolean;
  even: boolean;
}

export class BitcoinMasterModel extends BaseMonitoringState {
  result: BitcoinMasterResultModel;
}
