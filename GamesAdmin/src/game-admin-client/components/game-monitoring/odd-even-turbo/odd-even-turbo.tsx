import BigSmallBase from "../big-small-base/big-small-base";
import { types } from "../game-monitoring-config";

export default class OddEvenTurbo extends BigSmallBase {
  get turbo(): boolean {
    return true;
  }

  get type() {
    return types.clturbo;
  }

  get totalTimeInRound(): number {
    return 30;
  }

  get waitingTime(): number {
    return 5;
  }
}
