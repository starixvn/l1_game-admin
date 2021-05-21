export enum SystemId {
  None,
  SC,
  SM,
}

export default class BetItem {
  name: string;
  bet: number;
  time: string;
  choice: string;
  system: SystemId;

  constructor(
    name: string = "",
    bet: number = 0,
    time: string = "",
    choice = "",
    system = SystemId.None
  ) {
    this.name = name;
    this.bet = bet;
    this.time = time;
    this.choice = choice;
    this.system = system;
  }
}
