import { KenoMatchModel } from "../../common/model/KenoMatchModel";
import { GameState } from "../../shared/common/GameState";
import KenoMatch from "./keno-match-base";

export class KenoMatch1 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match";
  }

  umHandler(data: any): void {
    if (this.isFirstLoad || this.state.time.remainingTime == 27) {
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
    const waitingTime = 6;
    const { result } = this.state;
    result.fullTime = finish;
    this.setState({
      result,
      gameState: GameState.waiting,
      time: {
        remainingTime: waitingTime,
      },
    });

    setTimeout(() => {
      const newState = new KenoMatchModel();

      this.setState({
        ...newState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        time: {
          remainingTime: 27,
        },
      });
    }, (waitingTime + 1) * 1000);
  }

  remainingTimeHandler(data): void {
    const { result } = this.state;

    if (data.remainingTime > 56) {
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
          remainingTime: data.remainingTime - 56,
        },
      });

      this.timeout = setTimeout(() => {
        const newState = new KenoMatchModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: 27,
          },
        });
      }, (data.remainingTime - 55) * 1000);
    } else {
      if (data.remainingTime > 28) {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: data.remainingTime - 28,
          },
        });
      } else {
        if (data.remainingTime > 18 && data.remainingTime <= 28) {
          result.firstHalf = data.firstHalf;
          this.setState({
            gameState: GameState.waiting,
            roundNumber: data.roundId,
            result,
            time: {
              remainingTime: data.remainingTime - 19,
            },
          });

          this.timeout = setTimeout(() => {
            this.setState({
              gameState: GameState.running,
              roundNumber: data.roundId,
              time: {
                remainingTime: 18,
              },
            });
          }, (data.remainingTime - 18) * 1000);
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
      const waitingTime = 8;
      const { result } = this.state;
      result.firstHalf.result.blue = data.firstHalfResult.result.blue;
      result.firstHalf.result.red = data.firstHalfResult.result.red;
      result.firstHalf.settlementResult = data.firstHalfResult.settlementResult;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          time: {
            remainingTime: 18,
          },
        });
      }, (waitingTime + 1) * 1000);
    }
  }
}

export class KenoMatch2 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match MD5";
  }

  umHandler(data: any): void {
    if (this.isFirstLoad || this.state.time.remainingTime == 27) {
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
    const waitingTime = 11;
    const { result } = this.state;
    result.fullTime = finish;
    this.setState({
      result,
      gameState: GameState.waiting,
      time: {
        remainingTime: waitingTime,
      },
    });

    setTimeout(() => {
      const newState = new KenoMatchModel();

      this.setState({
        ...newState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        time: {
          remainingTime: 27,
        },
      });
    }, (waitingTime + 1) * 1000);
  }

  remainingTimeHandler(data): void {
    {
      const { result } = this.state;

      if (data.remainingTime > 60) {
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
            remainingTime: data.remainingTime - 60,
          },
        });

        this.timeout = setTimeout(() => {
          const newState = new KenoMatchModel();

          this.setState({
            ...newState,
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 27,
            },
          });
        }, (data.remainingTime - 59) * 1000);
      } else {
        if (data.remainingTime > 32) {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: data.remainingTime - 32,
            },
          });
        } else {
          if (data.remainingTime > 18 && data.remainingTime <= 32) {
            result.firstHalf = data.firstHalf;
            this.setState({
              gameState: GameState.waiting,
              roundNumber: data.roundId,
              result,
              time: {
                remainingTime: data.remainingTime - 19,
              },
            });

            this.timeout = setTimeout(() => {
              this.setState({
                gameState: GameState.running,
                roundNumber: data.roundId,
                time: {
                  remainingTime: 18,
                },
              });
            }, (data.remainingTime - 18) * 1000);
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
      const waitingTime = 12;
      const { result } = this.state;
      result.firstHalf.result.blue = data.firstHalfResult.result.blue;
      result.firstHalf.result.red = data.firstHalfResult.result.red;
      result.firstHalf.settlementResult = data.firstHalfResult.settlementResult;
      this.setState({
        result,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          time: {
            remainingTime: 18,
          },
        });
      }, (waitingTime + 1) * 1000);
    }
  }
}
