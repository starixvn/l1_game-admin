import TimeModel from "./TimeModel";
import { GameState } from "../../shared/common/GameState";

export default class RouletteModel {
    roundNumber: number;
    time: TimeModel;
    result: number;
    gameState: number;
    isShowResultUpdater: boolean;

    constructor() {
        this.result = -1;
        this.time = new TimeModel();
        this.gameState = GameState.none;
        this.isShowResultUpdater = false;
    }
}