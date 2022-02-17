import React from "react";
import _ from "lodash";
import { BetChoice } from "./shared/model";
import GameTemplate from "../game-template/game-template";
import MonitorBoard from "./shared/monitor-board";
import BetTable from "./shared/bet-table";
import KenoResult from "../../shared/keno-result/keno-result";
import style from "./base.module.scss";
import KenoResultUpdater from "../../shared/keno-result-updater/keno-result-updater";
import gameSettings from "./game-settings";
import { SystemId } from "../../common/model/BetItem";

type Props = {
  onMessage: Function;
  sendMessage: Function;
  gameName: string;
  sizeOfNumbers: number;
  isProduction: boolean;
  isMini?: boolean;
  partners: Array<string>;
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
  systemFilter: SystemId;
  partnerFilter: string;
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
  inGame: { [username: string]: { [box: string]: any } };
  finish: {
    result: Array<number>;
    roundId: number;
    settlementResult: { [box: string]: boolean };
    time: Date;
  };
  exchangeRates: any;
  currencies: any;
  result: Array<number>;
  settlementResult: { [box: string]: boolean };
  choices: Array<string>;
};

export default class extends React.Component<Props, State> {
  private timeout: NodeJS.Timeout;
  private isFirstLoad: boolean;
  private currencyExchangeRate: Object;
  private betList: Array<BetChoice>;

  constructor(props: Props) {
    super(props);

    this.isFirstLoad = true;
    this.currencyExchangeRate = {};
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
      systemFilter: SystemId.None,
      partnerFilter: "",
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
            this.reset();
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
        handle: () => {
          this.props.sendMessage({ sync: true });

          this.setState({
            remainingTime: this.state.durations.wholeRound + 1,
          });

          this.timeout = setTimeout(() => {
            this.setState({
              roundId: this.state.roundId + 1,
              remainingTime: this.state.durations.placingBets,
            });
          }, (this.state.durations.wholeRound - this.state.durations.placingBets + 1) * 1000);
        },
      },
      {
        selector: () => message.inGame,
        handle: () => {
          const { inGame } = message;
          const { systemFilter, partnerFilter } = this.state;

          this.betList = this.state.choices.map((choice) => {
            const bets = Object.values(inGame)
              .reverse()
              .map((bet) => {
                const rate = this.currencyExchangeRate[bet.currency] || 0;
                const baseBetAmount = parseFloat(
                  (bet.betTracks[choice.name] * rate).toFixed(2)
                );
                return {
                  name: bet.name.toString(),
                  bet: baseBetAmount,
                  partnerId: bet.partnerId,
                  system: bet.system,
                };
              })
              .filter((betOfUser) => betOfUser.bet > 0);

            choice.bets = bets;
            return choice;
          });

          this.setState({
            choices: this.getBetListByFilter(systemFilter, partnerFilter),
          });
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
      {
        selector: () => message.exchangeRates,
        handle: () => {
          const { exchangeRates, currencies } = message;
          const exchangeRateMapper = currencies.reduce((result, currency) => {
            const multiplier = currency.multiplier;
            const exchangeRate = exchangeRates.code[currency.code]
              ? exchangeRates.code[currency.code].$numberDecimal
              : 1;

            result[currency.name] = multiplier * exchangeRate;

            return result;
          }, {});

          this.currencyExchangeRate = exchangeRateMapper;
        },
      },
    ];

    actions.forEach((action) => action.selector() && action.handle());
  }

  getBetListByFilter(system, partnerId) {
    const filterBetList = _.cloneDeep(this.betList);

    filterBetList.forEach((choice) => {
      choice.bets = choice.bets.filter(
        (bet) =>
          (!system || bet.system === system) &&
          (!partnerId || bet.partnerId === partnerId)
      );
    });

    return filterBetList;
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

    const onChangePartnerFilter = (partnerId) => {
      const { systemFilter } = this.state;

      const newBetList = this.getBetListByFilter(systemFilter, partnerId);

      this.setState({
        partnerFilter: partnerId,
        choices: newBetList,
      });
    };

    const onChangeSystemFilter = (systemFilter) => {
      const { partnerFilter } = this.state;

      const newBetList = this.getBetListByFilter(systemFilter, partnerFilter);

      this.setState({
        systemFilter: systemFilter,
        choices: newBetList,
      });
    };

    return (
      <div>
        <GameTemplate
          renderMonitor={renderMonitor}
          renderBetList={renderBetList}
          onChangeSystemFilter={onChangeSystemFilter}
          onChangePartnerFilter={onChangePartnerFilter}
          enableFilter={true}
          disabled={this.state.disabled}
          partners={this.props.partners}
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
