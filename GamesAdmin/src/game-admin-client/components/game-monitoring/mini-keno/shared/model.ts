import { SystemId } from "../../../common/model/BetItem";

export interface BetOfUser {
  name: string;
  bet: number;
  system: SystemId;
  partnerId: string;
}

export interface IBetChoice {
  name: string;
  bets: Array<BetOfUser>;

  total(): number;
}

export class BetChoice implements IBetChoice {
  public bets: Array<BetOfUser>;
  public name: string;
  public isWin: boolean;
  public type: string;

  constructor(bets: Array<BetOfUser>, name: string, isWin: boolean) {
    this.bets = bets;
    this.name = name;
    this.isWin = isWin;
    this.type = `keno${name}`;
  }

  total(): number {
    return this.bets.reduce((result, { bet }) => result + bet, 0);
  }
}
