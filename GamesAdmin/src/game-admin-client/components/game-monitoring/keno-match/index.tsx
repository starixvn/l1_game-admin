import { KenoMatchModel } from "../../common/model/KenoMatchModel";
import { GameState } from "../../shared/common/GameState";
import KenoMatch from "./keno-match-base";

export class KenoMatch1 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match 1";
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 54) {
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

    if (!!data.inGame && Object.keys(data.inGame).length > 0) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.finishHalfTime) {
      this.isFirstLoad = false;
      const waitingTime = 8;
      const { result } = this.state;
      result.halfTime = data.finishHalfTime;
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

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 7;
      const { result } = this.state;
      result.fullTime = data.finish;
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

    if (!!data.remainingTime) {
      const { result } = this.state;

      if (data.remainingTime > 54) {
        result.halfTime = data.halfTime;
        result.fullTime = data.fullTime;
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result,
          time: {
            remainingTime: data.remainingTime - 54,
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
        }, (data.remainingTime - 53) * 1000);
      } else {
        if (data.remainingTime > 27) {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: data.remainingTime - 27,
            },
          });
        } else {
          if (data.remainingTime > 18 && data.remainingTime <= 27) {
            result.halfTime = data.halfTime;
            this.setState({
              gameState: GameState.waiting,
              roundNumber: data.roundId,
              result,
              time: {
                remainingTime: data.remainingTime - 18,
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
            result.halfTime = data.halfTime;
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
}

export class KenoMatch2 extends KenoMatch {
  constructor(props) {
    super(props);
    this.gameName = "Keno Match 2";
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 58) {
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

    if (!!data.inGame && Object.keys(data.inGame).length > 0) {
      this.setState({
        betList: this.getUserBetList(data.inGame, this.state.betList),
      });
    }

    if (!!data.finishHalfTime) {
      this.isFirstLoad = false;
      const waitingTime = 6;
      const { result } = this.state;
      result.halfTime = data.finishHalfTime;
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

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 11;
      const { result } = this.state;
      result.fullTime = data.finish;
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

    if (!!data.remainingTime) {
      const { result } = this.state;

      if (data.remainingTime > 58) {
        result.halfTime = data.halfTime;
        result.fullTime = data.fullTime;
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result,
          time: {
            remainingTime: data.remainingTime - 58,
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
        }, (data.remainingTime - 57) * 1000);
      } else {
        if (data.remainingTime > 31) {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: data.remainingTime - 27,
            },
          });
        } else {
          if (data.remainingTime > 18 && data.remainingTime <= 31) {
            result.halfTime = data.halfTime;
            this.setState({
              gameState: GameState.waiting,
              roundNumber: data.roundId,
              result,
              time: {
                remainingTime: data.remainingTime - 18,
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
            result.halfTime = data.halfTime;
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
}
