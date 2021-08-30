import { KenoPowerModel } from "../../common/model/KenoPowerModel";
import { GameState } from "../../shared/common/GameState";
import KenoPower from "./keno-power-base";

export class KenoPower1 extends KenoPower {
  constructor(props) {
    super(props);
    this.gameName = "Keno Power";
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 52) {
        clearTimeout(this.timeout);
        const model = new KenoPowerModel();
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

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 7;

      this.setState({
        result: data.finish.result,
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new KenoPowerModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 42,
          },
        });
      }, (waitingTime + 1) * 1000);
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 43) {
        this.setState((prevState) => ({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? data.result : prevState.result,
          time: {
            remainingTime: data.remainingTime - 43,
          },
        }));

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 42,
            },
          });
        }, (data.remainingTime - 42) * 1000);
      } else {
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

export class KenoPower2 extends KenoPower {
  constructor(props) {
    super(props);
    this.gameName = "Keno Power MD5";
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 52) {
        clearTimeout(this.timeout);
        const model = new KenoPowerModel();
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

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 11;

      this.setState({
        result: data.finish.result,
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime,
        },
      });

      setTimeout(() => {
        const newState = new KenoPowerModel();

        this.setState({
          ...newState,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 42,
          },
        });
      }, (waitingTime + 1) * 1000);
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 43) {
        this.setState((prevState) => ({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? data.result : prevState.result,
          time: {
            remainingTime: data.remainingTime - 43,
          },
        }));

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 42,
            },
          });
        }, (data.remainingTime - 42) * 1000);
      } else {
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
