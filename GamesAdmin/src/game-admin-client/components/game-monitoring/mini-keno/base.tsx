import React from "react";
import { BetChoice } from "./shared/model";
import GameTemplate from "../game-template/game-template";
import MonitorBoard from "./shared/monitor-board";
import BetTable from "./shared/bet-table";
import KenoResult from "../../shared/keno-result/keno-result";
import style from "./base.module.scss";
import KenoResultUpdater from "../../shared/keno-result-updater/keno-result-updater";

type Props = {
  onMessage: Function;
  sendMessage: Function;
  gameName: string;
  sizeOfNumbers: number;
  isProduction: boolean;
  isMini?: boolean;
};

type State = {
  remainingTime: number;
  roundId: number;
  choices: Array<BetChoice>;
  durations: {
    wholeRound: number;
    placingBets: number;
    lockingBet: number;
  };
  resultText: string;
  result: Array<number>;
  boxes: Array<string>;
  isShowSetResultPanel: boolean;
};

type KenoMessage = {
  totalBets: { [key: string]: number };
  remainingTime: number;
  roundId: number;
  durations: {
    wholeRound: number;
    placingBets: number;
    lockingBet: number;
  };
  ingame: { [username: string]: { [box: string]: number } };
  finish: {
    result: Array<number>;
    roundId: number;
    settlementResult: { [box: string]: boolean };
    time: Date;
  };
  choices: Array<string>;
};

export default class extends React.Component<Props, State> {
  constructor(props: Props) {
    super(props);

    this.state = {
      remainingTime: 0,
      roundId: 0,
      choices: [],
      durations: {
        wholeRound: 0,
        placingBets: 0,
        lockingBet: 0,
      },
      resultText: "",
      result: [],
      boxes: null,
      isShowSetResultPanel: false,
    };

    this.showSetResultPanel = this.showSetResultPanel.bind(this);
    this.renderSettlementResult = this.renderSettlementResult.bind(this);
    this.onReceiveMessage = this.onReceiveMessage.bind(this);
    this.reset = this.reset.bind(this);
    this.updaterSubmit = this.updaterSubmit.bind(this);
  }

  componentDidMount() {
    this.props.onMessage(this.onReceiveMessage);
  }

  onReceiveMessage(message: KenoMessage) {
    const actions = [
      {
        selector: () => message.durations,
        handle: () => this.setState({ durations: message.durations }),
      },
      {
        selector: () => message.remainingTime,
        handle: () => {
          const { durations } = this.state;
          const { remainingTime } = message;

          if (message.remainingTime > durations.placingBets) {
            this.setState({
              roundId: message.roundId - 1,
              remainingTime: remainingTime + 1
            });

            setTimeout(() => {
              this.setState({
                roundId: this.state.roundId + 1,
                remainingTime: durations.placingBets
              });
            }, (remainingTime - durations.placingBets + 1) * 1000);
          } else {
            this.setState({
              roundId: message.roundId,
              remainingTime: message.remainingTime
            });
          }
        },
      },
      // {
      //   selector: () => message.finish,
      //   handle: () => this.props.sendMessage({ sync: true }),
      // },
      {
        selector: () => message.ingame && message.totalBets,
        handle: () => {
          const { ingame } = message;
          const choices = this.state.choices.map((choice) => {
            const bets = Object.values(ingame)
              .reverse()
              .map((bet) => ({
                name: bet.name.toString(),
                bet: bet.betTracks[choice.name],
              }))
              .filter((betOfUser) => betOfUser.bet > 0);

            choice.bets = bets;
            return choice;
          });

          this.setState({ choices });
        },
      },
      {
        selector: () => message.finish,
        handle: () => {
          const { settlementResult, result } = message.finish;
          const { durations } = this.state;
          const choices = this.state.choices.map((choice) => {
            choice.isWin = settlementResult[choice.name];
            return choice;
          });
          const resultText = this.state.choices
            .reduce(
              (result, choice) =>
                choice.isWin
                  ? `${result} - ${choice.name.toUpperCase()}`
                  : result,
              ""
            )
            .substr(3);

          this.setState({ choices, result, resultText, remainingTime: durations.wholeRound + 1 });

          setTimeout(() => {
            this.setState({
              roundId: this.state.roundId + 1,
              remainingTime: durations.placingBets
            });
          }, (durations.wholeRound - durations.placingBets + 1) * 1000 + 500);
        },
      },
      {
        selector: () => message.choices && !this.state.boxes,
        handle: () => {
          const choices = message.choices.map(
            (choice) => new BetChoice([], choice, false)
          );
          this.setState({ boxes: message.choices, choices });
        },
      },
    ];

    actions.forEach((action) => action.selector() && action.handle());
  }

  reset() {
    const choices = this.state.choices.map((choice) => {
      choice.bets = [];
      choice.isWin = false;
      return choice;
    });

    this.setState({ choices, result: [], resultText: "" });
  }

  showSetResultPanel() {
    this.setState({ isShowSetResultPanel: !this.state.isShowSetResultPanel });
  }

  renderSettlementResult() {
    const total = this.state.result.reduce((a, b) => a + b, 0);
    return (
      <>
        <KenoResult
          listResult={this.state.result}
          maxLength={this.props.sizeOfNumbers}
        />
        <div className={style.result}>{!!total && `Total: ${total}`}</div>
        <div className={style.result}>{this.state.resultText}</div>
      </>
    );
  }

  updaterSubmit(list: Array<number>, listLabel: Array<string>) {
    const resultText = listLabel
      .reduce((result, label) => `${result} - ${label.toUpperCase()}`, "")
      .substr(3);
    this.setState({ result: list, resultText, isShowSetResultPanel: false });
    this.props.sendMessage({
      setResult: { result: list, roundId: this.state.roundId },
    });
  }

  render() {
    const { remainingTime, roundId, choices, durations } = this.state;

    const { gameName } = this.props;

    const monitorProps = {
      gameName,
      roundId,
      remainingTime,
      choices,
      renderSettlementResult: this.renderSettlementResult,
      showSetResultPanel: this.showSetResultPanel,
      placingBetTime: durations.placingBets,
      lockingBetTime: durations.lockingBet,
      reset: this.reset,
    };

    const betTableProps = {
      choices,
    };

    return (
      <div>
        <GameTemplate
          renderMonitor={React.createElement(MonitorBoard, monitorProps)}
          renderBetList={React.createElement(BetTable, betTableProps)}
        />
        {!this.props.isProduction && (
          <KenoResultUpdater
            modal={this.state.isShowSetResultPanel}
            toggle={this.showSetResultPanel}
            submit={this.updaterSubmit}
            isMini={this.props.isMini}
          />
        )}
      </div>
    );
  }
}
