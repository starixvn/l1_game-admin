import React from 'react';
import {BetChoice, BetOfUser} from "./shared/model";
import GameTemplate from "../game-template/game-template";
import MonitorBoard from './shared/monitor-board';
import BetTable from './shared/bet-table';
import Logger from '../../util/logger';

type Props = {
  onMessage: Function,
  sendMessage: Function,
  gameName: string,
};

type State = {
  remainingTime: number
  roundId: number
  choices: Array<BetChoice>
  durations: {
    wholeRound: number,
    placingBets: number,
    lockingBet: number,
  }
}

type KenoMessage = {
  totalBets: {[key: string]: number}
  remainingTime: number,
  roundId: number,
  durations: {
    wholeRound: number,
    placingBets: number,
    lockingBet: number
  },
  ingame: {[username: string]: {[box: string]: number}},
  finish: {
    result: Array<number>,
    roundId: number,
    settlementResult: {[box: string]: boolean},
    time: Date,
  }
}

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
    }

    this.showSetResultPanel = this.showSetResultPanel.bind(this);
    this.renderSettlementResult = this.renderSettlementResult.bind(this);
    this.onReceiveMessage = this.onReceiveMessage.bind(this);
    this.reset = this.reset.bind(this);
  }

  componentDidMount() {
    this.props.onMessage(this.onReceiveMessage);
  }

  onReceiveMessage(message: KenoMessage) {
    const actions = [
      {
        selector: () => message.durations,
        handle: () => this.setState({durations: message.durations}),
      },
      {
        selector: () => message.roundId,
        handle: () => this.setState(Object.assign({}, this.state, message)),
      },
      {
        selector: () => message.finish,
        handle: () => this.props.sendMessage({sync: true}),
      },
      {
        selector: () => message.ingame && message.totalBets,
        handle: () => {
          const {ingame, totalBets} = message;
          const boxes = Object.keys(totalBets);
          const choices = boxes.map(box => {
            const bets = Object.values(ingame)
              .map(bet => ({
                name: bet.name.toString(),
                bet: bet[box],
              }))
              .filter(betOfUser => betOfUser.bet > 0);

            return new BetChoice(bets, box, false);
          });

          this.setState({choices});
        }
      },
      {
        selector: () => message.finish,
        handle: () => {
          const {settlementResult} = message.finish;
          const choices = this.state.choices.map(choice => {
            choice.isWin = settlementResult[choice.name];
            return choice;
          });

          this.setState({choices});
        }
      }
    ];

    actions.forEach(action => action.selector() && action.handle());
  }

  reset() {
    const choices = this.state.choices.map(choice => {
      choice.bets = [];
      choice.isWin = false;
      return choice;
    });

    this.setState({choices});
  }

  showSetResultPanel() {
    Logger.info('Show set result panel');
  }

  renderSettlementResult() {
    return <p>Settlement Result Here</p>
  }

  render() {
    const {
      remainingTime,
      roundId,
      choices,
      durations,
    } = this.state;

    const monitorProps = {
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
      </div>
    )
  }
}
