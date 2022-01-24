import { SystemId } from "./BetItem";
import TimeModel from "./TimeModel";

export default class BaseMonitoringState {
  roundNumber: number;
  time: TimeModel;
  betList: any;
  gameState: number;
  disabled: boolean;
  isOpenControlResultModal: boolean;
  isHandleNextRoundResult: boolean;
  currencyExchangeRate: { [key: string]: number };
  result: any;
  settlementResult: any;
  systemFilter: SystemId;
  partnerFilter: string;

  constructor() {
    this.betList = {};
    this.time = new TimeModel();
    this.gameState = 0;
    this.disabled = false;
    this.isOpenControlResultModal = false;
    this.isHandleNextRoundResult = false;
    this.currencyExchangeRate = {};
  }
}

export { BaseMonitoringState };
