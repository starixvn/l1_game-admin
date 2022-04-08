import React from "react";
import BaseMonitoringState from "../../common/model/BaseMonitoringModel";
import BetItem from "../../common/model/BetItem";
import { GameState } from "../../shared/common/GameState";
import GameTemplate from "../../game-monitoring/game-template/game-template";

type BaseMonitoringProps = {
  onMessage: Function;
  sendMessage: Function;
  isProduction: boolean;
  partners: Array<string>;
};

export default class BaseMonitoring<
  GameStateModel extends BaseMonitoringState
> extends React.Component<BaseMonitoringProps, GameStateModel> {
  protected isFirstLoad: boolean;
  protected timeout: NodeJS.Timeout;
  protected currencyExchangeRate: Object;
  protected betList: any;

  constructor(props) {
    super(props);

    this.betList = this.defaultBetList;
    this.isFirstLoad = true;
    this.renderMonitor = this.renderMonitor.bind(this);
    this.renderBetList = this.renderBetList.bind(this);
    this.onChangePartnerFilter = this.onChangePartnerFilter.bind(this);
    this.onChangeSystemFilter = this.onChangeSystemFilter.bind(this);
  }

  get defaultState() {
    const baseState = new BaseMonitoringState();

    return {
      ...baseState,
      betList: this.defaultBetList,
      result: this.defaultResult,
      settlementResult: this.defaultSettlementResult,
    };
  }

  get defaultResult() {
    return null;
  }

  get defaultSettlementResult() {
    return null;
  }

  get totalTimeInRound() {
    return 0;
  }

  get waitingTime() {
    return 0;
  }

  get defaultBetList() {
    return {};
  }

  get isExcludeUUS(): boolean {
    return false;
  }

  getChoiceDisplay(key) {
    return null;
  }
  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  umHandler(data) {
    if (
      this.isFirstLoad ||
      this.state.time.remainingTime == this.totalTimeInRound
    ) {
      clearTimeout(this.timeout);
      this.setState({
        time: { remainingTime: 0 },
        result: this.defaultResult,
        settlementResult: this.defaultSettlementResult,
        roundNumber: this.isFirstLoad
          ? this.state.roundNumber
          : this.state.roundNumber - 1,
        gameState: GameState.disabled,
      });
      this.isFirstLoad = false;
    }
  }

  inGameHandler({ inGame }) {
    const { partnerFilter, systemFilter } = this.state;
    this.betList = this.getUserBetList(inGame, this.state.betList);

    this.setState({
      betList: this.getBetListByFilter(systemFilter, partnerFilter),
    });
  }

  remainingTimeHandler({ remainingTime, roundId, result }) {
    this.setState({ disabled: false });
    if (remainingTime > this.totalTimeInRound) {
      this.setState({
        gameState: GameState.waiting,
        roundNumber: roundId - 1,
        time: {
          remainingTime: remainingTime - (this.totalTimeInRound + 1),
        },
        result: result ? this.getResult(result) : this.state.result,
      });

      setTimeout(() => {
        this.setState({
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: this.totalTimeInRound,
          },
          result: this.defaultResult,
        });
      }, (remainingTime - this.totalTimeInRound) * 1000);
    } else {
      this.setState({
        gameState: GameState.running,
        roundNumber: roundId,
        time: {
          remainingTime: remainingTime,
        },
      });
    }
  }

  finishHandler({ finish }) {
    this.isFirstLoad = false;

    this.setState({
      result: this.getResult(finish.result),
      settlementResult: this.getSettlementResult(finish.settlementResult),
      gameState: GameState.waiting,
      time: {
        remainingTime: this.waitingTime,
      },
    });

    this.timeout = setTimeout(() => {
      this.setState({
        ...this.defaultState,
        gameState: GameState.running,
        roundNumber: this.state.roundNumber + 1,
        time: {
          remainingTime: this.totalTimeInRound,
        },
      });
    }, this.waitingTime * 1000);
  }

  exchangeRateHandler({ exchangeRates, currencies }) {
    const exchangeRateMapper = currencies.reduce((result, currency) => {
      const multiplier = currency.multiplier;
      const exchangeRate = exchangeRates.code[currency.code]
        ? exchangeRates.code[currency.code].$numberDecimal
        : 1;

      result[currency.name] = multiplier * exchangeRate;

      return result;
    }, {});

    this.currencyExchangeRate = exchangeRateMapper;
  }

  handleData(data) {
    if (!!data.um) {
      this.umHandler(data);
    }

    if (!!data.inGame) {
      this.inGameHandler(data);
    }

    if (!!data.remainingTime) {
      this.remainingTimeHandler(data);
    }

    if (!!data.finish) {
      this.finishHandler(data);
    }

    if (!!data.exchangeRates) {
      this.exchangeRateHandler(data);
    }
  }

  getResult(result) {
    return result;
  }

  getSettlementResult(settlementResult) {
    return settlementResult;
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach((key) => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      const userRate = this.currencyExchangeRate[data[user].currency];
      let betTracks = data[user].betTracks;

      if (userRate) {
        Object.keys(betTracks).forEach((key) => {
          const betAmount = betTracks[key];
          if (betAmount) {
            const baseBetAmount = parseFloat((betAmount * userRate).toFixed(2));
            !this.isExcludeUUS && (currentData[key].total += baseBetAmount);

            currentData[key].dataList.unshift(
              new BetItem(
                data[user].name,
                baseBetAmount,
                null,
                this.getChoiceDisplay(key),
                data[user].system,
                data[user].currency,
                data[user].partnerId
              )
            );
          }
        });
      }
    }

    return { ...currentData };
  }

  onChangeSystemFilter(system) {
    const { partnerFilter } = this.state;

    const newBetList = this.getBetListByFilter(system, partnerFilter);

    this.setState({
      systemFilter: system,
      betList: newBetList,
    });
  }

  onChangePartnerFilter(partnerId) {
    const { systemFilter } = this.state;

    const newBetList = this.getBetListByFilter(systemFilter, partnerId);

    this.setState({
      partnerFilter: partnerId,
      betList: newBetList,
    });
  }

  getBetListByFilter(system, partnerId) {
    return Object.keys(this.betList).reduce((result, betChoice) => {
      result[betChoice] = {
        dataList: this.betList[betChoice].dataList.filter(
          (bet) =>
            (!system || bet.system === system) &&
            (!partnerId || bet.partnerId === partnerId)
        ),
      };

      result[betChoice].total = result[betChoice].dataList.reduce(
        (sum, bet) => sum + bet.bet,
        0
      );

      return result;
    }, {});
  }

  toggleModal() {
    this.setState({
      isOpenControlResultModal: !this.state.isOpenControlResultModal,
    });
  }

  renderGameControl() {
    return <></>;
  }

  renderMonitor() {
    return <></>;
  }

  renderBetList() {
    return <></>;
  }

  render() {
    return (
      <div>
        {!this.props.isProduction && this.renderGameControl()}
        <GameTemplate
          renderBetList={this.renderBetList}
          renderMonitor={this.renderMonitor}
          onChangePartnerFilter={this.onChangePartnerFilter}
          onChangeSystemFilter={this.onChangeSystemFilter}
          disabled={this.state.disabled}
          enableFilter={true}
          partners={this.props.partners}
        ></GameTemplate>
      </div>
    );
  }
}
