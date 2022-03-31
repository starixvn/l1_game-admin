import { KenoMatchModel } from "../../common/model/KenoMatchModel";
import { GameState } from "../../shared/common/GameState";
import KenoMatch from "./keno-match-base";

const KENOMATCH1_DURATION = {
  wholeRound: 77,
  startFirstHalf: 71,
  showFirstHalfResult: 35,
  settleFirstHalf: 9,
  settleFullTime: 8,

  firstHalfPlacingBets: 35,
  placingBets: 25,
};

const KENOMATCH2_DURATION = {
  wholeRound: 85,
  startFirstHalf: 74,
  showFirstHalfResult: 39,
  settleFirstHalf: 13,
  settleFullTime: 12,

  firstHalfPlacingBets: 35,
  placingBets: 25,
};

export class KenoMatch1 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match";
  }

  umHandler(data: any): void {
    if (
      this.isFirstLoad ||
      this.state.time.remainingTime == KENOMATCH1_DURATION.firstHalfPlacingBets
    ) {
      clearTimeout(this.timeout);
      const model = new KenoMatchModel();
      this.setState({
        time: { remainingTime: 0 },
        gameState: GameState.disabled,
        roundNumber: this.isFirstLoad
          ? this.state.roundNumber
          : this.state.roundNumber - 1,
        result: model.result,
      });
      this.isFirstLoad = false;
    }
  }

  finishHandler({ finish }: { finish: any }): void {
    this.isFirstLoad = false;
    const { result } = this.state;
    result.fullTime = finish;
    this.setState({
      result,
      gameState: GameState.waiting,
      time: {
        remainingTime: KENOMATCH1_DURATION.settleFullTime,
      },
    });

    setTimeout(() => {
      const newState = new KenoMatchModel();

      this.setState({
        ...newState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        time: {
          remainingTime: KENOMATCH1_DURATION.firstHalfPlacingBets,
        },
      });
    }, (KENOMATCH1_DURATION.settleFullTime + 1) * 1000);
  }

  remainingTimeHandler(data): void {
    const { result } = this.state;

    if (data.remainingTime > KENOMATCH1_DURATION.startFirstHalf) {
      if (data.firstHalf) {
        result.firstHalf = data.firstHalf;
      }
      if (data.fullTime) {
        result.fullTime = data.fullTime;
      }
      this.setState({
        gameState: GameState.waiting,
        roundNumber: data.roundId - 1,
        result,
        time: {
          remainingTime:
            data.remainingTime - KENOMATCH1_DURATION.startFirstHalf,
        },
      });

      this.timeout = setTimeout(() => {
        const newState = new KenoMatchModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: KENOMATCH1_DURATION.firstHalfPlacingBets,
          },
        });
      }, (data.remainingTime - (KENOMATCH1_DURATION.startFirstHalf - 1)) * 1000);
    } else {
      if (data.remainingTime > KENOMATCH1_DURATION.showFirstHalfResult) {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime:
              data.remainingTime - KENOMATCH1_DURATION.showFirstHalfResult,
          },
        });
      } else {
        if (
          data.remainingTime > KENOMATCH1_DURATION.placingBets &&
          data.remainingTime <= KENOMATCH1_DURATION.showFirstHalfResult
        ) {
          result.firstHalf = data.firstHalf;
          this.setState({
            gameState: GameState.waiting,
            roundNumber: data.roundId,
            result,
            time: {
              remainingTime:
                data.remainingTime - (KENOMATCH1_DURATION.placingBets + 1),
            },
          });

          this.timeout = setTimeout(() => {
            this.setState({
              gameState: GameState.running,
              roundNumber: data.roundId,
              time: {
                remainingTime: KENOMATCH1_DURATION.placingBets,
              },
            });
          }, (data.remainingTime - KENOMATCH1_DURATION.placingBets) * 1000);
        } else {
          result.firstHalf = data.firstHalf;
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: data.remainingTime,
            },
          });
        }
      }
    }
  }

  handleData(data) {
    super.handleData(data);

    if (!!data.firstHalfResult) {
      this.isFirstLoad = false;
      const { result } = this.state;
      result.firstHalf.result.blue = data.firstHalfResult.result.blue;
      result.firstHalf.result.red = data.firstHalfResult.result.red;
      result.firstHalf.settlementResult = data.firstHalfResult.settlementResult;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: KENOMATCH1_DURATION.settleFirstHalf,
        },
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          time: {
            remainingTime: KENOMATCH1_DURATION.placingBets,
          },
        });
      }, (KENOMATCH1_DURATION.settleFirstHalf + 1) * 1000);
    }
  }
}

export class KenoMatch2 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match MD5";
  }

  umHandler(data: any): void {
    if (
      this.isFirstLoad ||
      this.state.time.remainingTime == KENOMATCH2_DURATION.firstHalfPlacingBets
    ) {
      clearTimeout(this.timeout);
      const model = new KenoMatchModel();
      this.setState({
        time: { remainingTime: 0 },
        gameState: GameState.disabled,
        roundNumber: this.isFirstLoad
          ? this.state.roundNumber
          : this.state.roundNumber - 1,
        result: model.result,
      });
      this.isFirstLoad = false;
    }
  }

  finishHandler({ finish }): void {
    this.isFirstLoad = false;
    const { result } = this.state;
    result.fullTime = finish;
    this.setState({
      result,
      gameState: GameState.waiting,
      time: {
        remainingTime: KENOMATCH2_DURATION.settleFullTime,
      },
    });

    setTimeout(() => {
      const newState = new KenoMatchModel();

      this.setState({
        ...newState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        time: {
          remainingTime: KENOMATCH2_DURATION.firstHalfPlacingBets,
        },
      });
    }, (KENOMATCH2_DURATION.settleFullTime + 1) * 1000);
  }

  remainingTimeHandler(data): void {
    {
      const { result } = this.state;

      if (data.remainingTime > KENOMATCH2_DURATION.startFirstHalf) {
        if (data.firstHalf) {
          result.firstHalf = data.firstHalf;
        }
        if (data.fullTime) {
          result.fullTime = data.fullTime;
        }
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result,
          time: {
            remainingTime:
              data.remainingTime - KENOMATCH2_DURATION.startFirstHalf,
          },
        });

        this.timeout = setTimeout(() => {
          const newState = new KenoMatchModel();

          this.setState({
            ...newState,
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: KENOMATCH2_DURATION.firstHalfPlacingBets,
            },
          });
        }, (data.remainingTime - (KENOMATCH2_DURATION.startFirstHalf - 1)) * 1000);
      } else {
        if (data.remainingTime > KENOMATCH2_DURATION.showFirstHalfResult) {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime:
                data.remainingTime - KENOMATCH2_DURATION.showFirstHalfResult,
            },
          });
        } else {
          if (
            data.remainingTime > KENOMATCH2_DURATION.placingBets &&
            data.remainingTime <= KENOMATCH2_DURATION.showFirstHalfResult
          ) {
            result.firstHalf = data.firstHalf;
            this.setState({
              gameState: GameState.waiting,
              roundNumber: data.roundId,
              result,
              time: {
                remainingTime:
                  data.remainingTime - KENOMATCH2_DURATION.placingBets - 1,
              },
            });

            this.timeout = setTimeout(() => {
              this.setState({
                gameState: GameState.running,
                roundNumber: data.roundId,
                time: {
                  remainingTime: KENOMATCH2_DURATION.placingBets,
                },
              });
            }, (data.remainingTime - KENOMATCH2_DURATION.placingBets) * 1000);
          } else {
            result.firstHalf = data.firstHalf;
            this.setState({
              gameState: GameState.running,
              roundNumber: data.roundId,
              time: {
                remainingTime: data.remainingTime,
              },
            });
          }
        }
      }
    }
  }

  handleData(data) {
    super.handleData(data);

    if (!!data.firstHalfResult) {
      this.isFirstLoad = false;
      const { result } = this.state;
      result.firstHalf.result.blue = data.firstHalfResult.result.blue;
      result.firstHalf.result.red = data.firstHalfResult.result.red;
      result.firstHalf.settlementResult = data.firstHalfResult.settlementResult;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: KENOMATCH2_DURATION.settleFirstHalf,
        },
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          time: {
            remainingTime: KENOMATCH2_DURATION.placingBets,
          },
        });
      }, (KENOMATCH2_DURATION.settleFirstHalf + 1) * 1000);
    }
  }
}
