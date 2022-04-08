import BigSmallBase from "../big-small-base/big-small-base";
import { types } from "../game-monitoring-config";

export default class BigSmallTurbo extends BigSmallBase {
  get turbo(): boolean {
    return false;
  }

  get type() {
    return types.chanle;
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 5;
  }
}
