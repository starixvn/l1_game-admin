export enum SystemId {
  None,
  SC,
  SM,
  Mini
}

export default class BetItem {
  name: string;
  bet: number;
  time: string;
  choice: string;
  system: SystemId;
  currency: string;

  constructor(
    name: string = "",
    bet: number = 0,
    time: string = "",
    choice = "",
    system = SystemId.None,
    currency = ""
  ) {
    this.name = name;
    this.bet = bet;
    this.time = time;
    this.choice = choice;
    this.system = system;
    this.currency = currency;
  }
}
