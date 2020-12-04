import React from 'react';
import styles from './index.module.scss';
import TimeCounter from '../../shared/time-counter/time-counter';
import TotalBet from '../../shared/total-bet/total-bet';
import { Col, Container, Row } from 'reactstrap';
import GameTemplate from '../game-template/game-template';
import BetList from '../../shared/bet-list/bet-list';
import Dice from '../../shared/dice/dice';
import DiceModel from '../../common/model/DiceModel';
import DiceType from '../../common/model/DiceType';
import BetItem from '../../common/model/BetItem';
import RoundNum from '../../shared/round-num/round-num';
import { GameState } from '../../shared/common/GameState';
import VipSeDieWarModel, { VipSeDieWarResult } from '../../common/model/VipSeDieWarModel';

type Props = {
  onMessage: Function
}

export default class VipSeDieWar extends React.Component<Props, VipSeDieWarModel> {
  private isFirstLoad: boolean;
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.isFirstLoad = true;
    this.state = new VipSeDieWarModel();
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      if (this.isFirstLoad || this.state.time.remainingTime == 30) {
        clearTimeout(this.timeout);
        this.setState({
          time: { remainingTime: 0 },
          gameState: GameState.disabled,
          roundNumber: this.isFirstLoad ? this.state.roundNumber : this.state.roundNumber - 1,
          result: new VipSeDieWarModel().result
        });

        this.isFirstLoad = false;
      }
    }

    if (!!data.ingame) {
      this.setState({
        betList: this.getUserBetList(data.ingame, this.state.betList)
      })
    }

    if (!!data.remainingTime) {
      if (data.remainingTime > 30) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          result: data.result ? this.getResult(data.result) : this.state.result,
          time: {
            remainingTime: data.remainingTime - 31
          }
        })

        this.timeout = setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 30
            }
          })
        }, (data.remainingTime - 30) * 1000)
      }
      else {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: data.remainingTime
          }
        });
      }
    }

    if (!!data.finish) {
      this.isFirstLoad = false;
      const waitingTime = 8;
      this.setState({
        result: this.getResult(data.finish.result),
        settlementResult: data.finish.settlementResult,
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime
        }
      });

      setTimeout(() => {
        this.setState({
          result: new VipSeDieWarModel().result,
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30
          }
        })
      }, (waitingTime + 4) * 1000)
    }
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach(key => {
      currentData[key].total = 0;
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      let betTracks = data[user].betTracks;
      Object.keys(betTracks).forEach(key => {
        if (betTracks[key]) {
          currentData[key].total += betTracks[key];
          currentData[key].dataList.unshift(new BetItem(data[user].name, betTracks[key]));
        }
      });
    }

    return currentData;
  }

  getResult(data) {
    const result = new VipSeDieWarResult();
    result.banker = [];
    result.player = []

    Object.keys(data.player).forEach(key => {
      result.player.push(new DiceModel(data.player[key], DiceType.XocXoc));
    });

    Object.keys(data.banker).forEach(key => {
      result.banker.push(new DiceModel(data.banker[key], DiceType.XocXoc));
    });

    return result;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;

    return (
      <Container className={styles['sedie-container']}>
        <Row>
          <Col align='center' className={styles['sedie-timmer']}>
            <TimeCounter remainingTime={time.remainingTime} gameState={gameState} />
            <div>
              <div className={styles.title}>Mini Se Die</div>
              <div className={styles.title}>Player</div>
              <Row className={styles.result}>
                <Dice diceModel={result.player[0]} isSmall={false} />
                <Dice diceModel={result.player[1]} isSmall={false} />
                <Dice diceModel={result.player[2]} isSmall={false} />
                <Dice diceModel={result.player[3]} isSmall={false} />
              </Row>
              <div className={styles.title}>Banker</div>
              <Row className={styles.result}>
                <Dice diceModel={result.banker[0]} isSmall={false} />
                <Dice diceModel={result.banker[1]} isSmall={false} />
                <Dice diceModel={result.banker[2]} isSmall={false} />
                <Dice diceModel={result.banker[3]} isSmall={false} />
              </Row>
              <RoundNum roundNumber={roundNumber} />
            </div>
          </Col>
          <Col align='center' className={styles['sedie-betList']}>

          </Col>
        </Row>
      </Container>
    )
  }

  renderBetList() {
    const { betList, settlementResult } = this.state;

    return (
      <React.Fragment>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div>Even</div>)]}
            type='simple'
            dataList={betList.even.dataList}
            isWin={settlementResult.even}
          />
        </Col>
      </React.Fragment>
    )
  }

  render() {
    return (
      <div>
        <GameTemplate renderBetList={this.renderBetList()} renderMonitor={this.renderMonitor()} disabled={this.state.gameState == GameState.disabled}></GameTemplate>
      </div>
    );
  }
}
