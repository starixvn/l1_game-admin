import React from "react";
import { BetChoice } from "./shared/model";
import GameTemplate from "../game-template/game-template";
import MonitorBoard from "./shared/monitor-board";
import BetTable from "./shared/bet-table";
import KenoResult from "../../shared/keno-result/keno-result";
import style from "./base.module.scss";
import KenoResultUpdater from "../../shared/keno-result-updater/keno-result-updater";
import gameSettings from "./game-settings";

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
  disabled: boolean;
};

type KenoMessage = {
  um: boolean;
  totalBets: { [key: string]: number };
  remainingTime: number;
  roundId: number;
  durations: {
    wholeRound: number;
    placingBets: number;
    lockingBet: number;
  };
  inGame: { [username: string]: { [box: string]: number } };
  finish: {
    result: Array<number>;
    roundId: number;
    settlementResult: { [box: string]: boolean };
    time: Date;
  };
  result: Array<number>;
  settlementResult: { [box: string]: boolean };
  choices: Array<string>;
};

export default class extends React.Component<Props, State> {
  private timeout: NodeJS.Timeout;
  private isFirstLoad: boolean;
  constructor(props: Props) {
    super(props);

    this.isFirstLoad = true;

    const choices = gameSettings.choices.map(
      (choice) => new BetChoice([], choice, false)
    );

    this.state = {
      remainingTime: 0,
      roundId: 0,
      choices,
      durations: gameSettings[props.gameName].durations,
      resultText: "",
      result: [],
      boxes: gameSettings.choices,
      isShowSetResultPanel: false,
      disabled: false,
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
        selector: () => message.um,
        handle: () => {
          const { remainingTime, durations } = this.state;
          if (this.isFirstLoad || remainingTime == durations.placingBets) {
            clearTimeout(this.timeout);
            this.setState({
              disabled: message.um,
              remainingTime: 0,
              roundId: this.isFirstLoad
                ? this.state.roundId
                : this.state.roundId - 1,
            });
            this.reset(0);
            this.isFirstLoad = false;
          }
        },
      },
      {
        selector: () => message.remainingTime,
        handle: () => {
          const { durations } = this.state;

          this.setState({ disabled: false });

          if (message.remainingTime > durations.placingBets) {
            const { result, settlementResult } = message;
            const choices = settlementResult
              ? this.updateWinningChoice(this.state.choices, settlementResult)
              : this.state.choices;
            this.setState({
              roundId: message.roundId - 1,
              remainingTime: message.remainingTime + 1,
              result: result ? result : this.state.result,
              choices: choices,
              resultText: this.getResultText(choices),
            });

            this.timeout = setTimeout(() => {
              this.setState({
                roundId: this.state.roundId + 1,
                remainingTime: durations.placingBets,
              });
            }, (message.remainingTime - durations.placingBets + 1) * 1000);
          } else {
            this.setState({
              roundId: message.roundId,
              remainingTime: message.remainingTime,
            });
          }
        },
      },
      {
        selector: () => message.finish,
        handle: () => this.props.sendMessage({ sync: true }),
      },
      {
        selector: () => message.inGame,
        handle: () => {
          const { inGame } = message;
          const choices = this.state.choices.map((choice) => {
            const bets = Object.values(inGame)
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
          this.isFirstLoad = false;
          const { settlementResult, result } = message.finish;
          const choices = this.updateWinningChoice(
            this.state.choices,
            settlementResult
          );
          const resultText = this.getResultText(choices);

          this.setState({ choices, result, resultText });
        },
      },
    ];

    actions.forEach((action) => action.selector() && action.handle());
  }

  updateWinningChoice(choices, settlementResult) {
    return choices.map((choice) => {
      choice.isWin = settlementResult[choice.name];
      return choice;
    });
  }

  getResultText(choices) {
    return choices
      .reduce(
        (result, choice) =>
          choice.isWin ? `${result} - ${choice.name.toUpperCase()}` : result,
        ""
      )
      .substr(3);
  }

  reset(time: number) {
    if (time === this.state.durations.placingBets && time !== 0) {
      const choices = this.state.choices.map((choice) => {
        choice.bets = [];
        choice.isWin = false;
        return choice;
      });

      this.setState({ choices, result: [], resultText: "" });
    }

    if (time === 0) {
      this.setState({ remainingTime: this.state.durations.wholeRound + 1 });

      this.timeout = setTimeout(() => {
        this.setState({
          roundId: this.state.roundId + 1,
          remainingTime: this.state.durations.placingBets + 1,
        });
      }, (this.state.durations.wholeRound - this.state.durations.placingBets + 1) * 1000);
    }
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
      set_result: { result: list, roundId: this.state.roundId },
    });
  }

  render() {
    const { remainingTime, roundId, choices, durations, disabled } = this.state;

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
      disabled,
    };

    const betTableProps = {
      choices,
    };

    const renderMonitor = () => {
      return React.createElement(MonitorBoard, monitorProps);
    };

    const renderBetList = () => {
      return React.createElement(BetTable, betTableProps);
    };

    return (
      <div>
        <GameTemplate
          renderMonitor={renderMonitor}
          renderBetList={renderBetList}
          disabled={this.state.disabled}
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
