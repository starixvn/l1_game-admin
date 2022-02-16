import KenoPower from "./keno-power-base";

export class KenoPower1 extends KenoPower {
  constructor(props) {
    super(props);
    this.gameName = "Keno Power";
  }

  get totalTimeInRound(): number {
    return 42;
  }

  get waitingTime(): number {
    return 7;
  }
}

export class KenoPower2 extends KenoPower {
  constructor(props) {
    super(props);
    this.gameName = "Keno Power MD5";
  }

  get totalTimeInRound(): number {
    return 42;
  }

  get waitingTime(): number {
    return 11;
  }
}
