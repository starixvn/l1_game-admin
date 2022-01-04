export enum SystemId {
  None,
  SC,
  SM,
  Mini,
}

export const Partners = {
  ONEworks1: "1W",
  SBO: "SPart",
};

export default class BetItem {
  name: string;
  bet: number;
  time: string;
  choice: string;
  system: SystemId;
  currency: string;
  partnerId: string;

  constructor(
    name: string = "",
    bet: number = 0,
    time: string = "",
    choice = "",
    system = SystemId.None,
    currency = "",
    partnerId = ""
  ) {
    this.name = name;
    this.bet = bet;
    this.time = time;
    this.choice = choice;
    this.system = system;
    this.currency = currency;
    this.partnerId = partnerId;
  }
}
