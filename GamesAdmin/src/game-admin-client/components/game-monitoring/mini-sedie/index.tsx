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
import MiniSedieModel from '../../common/model/MiniSedieModel';

type Props = {
  onMessage: Function
}

export default class ShakeThePlate extends React.Component<Props, MiniSedieModel> {
  private timeout: NodeJS.Timeout;
  constructor(props) {
    super(props);

    this.state = new MiniSedieModel();

  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.um) {
      clearTimeout(this.timeout);
      this.setState({
        disabled: data.um,
        time: { remainingTime: 0 },
        gameState: GameState.none
      });
    }

    if (!!data.ingame) {
      this.setState({
        betList: this.getUserBetList(data.ingame, this.state.betList)
      })
    }

    if (!!data.remainingTime) {
      this.setState({ disabled: false });
      if (data.remainingTime > 30) {
        this.setState({
          gameState: GameState.waiting,
          roundNumber: data.roundId - 1,
          time: {
            remainingTime: data.remainingTime - 31
          }
        })

        setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: data.roundId,
            time: {
              remainingTime: 30
            }
          })
        }, (data.remainingTime - 30) * 1000 + 500)
      }
      else {
        this.setState({
          gameState: GameState.running,
          roundNumber: data.roundId,
          time: {
            remainingTime: data.remainingTime
          }
        })
      }
    }

    if (!!data.finish) {
      const waitingTime = 9;
      this.setState({
        result: this.getResult(data.finish.result),
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime
        }
      });

      this.timeout = setTimeout(() => {
        this.setState({
          result: [
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc),
            new DiceModel(-1, DiceType.XocXoc)
          ],
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: 30
          }
        })
      }, waitingTime * 1000 + 500)
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
    const result = new Array<DiceModel>();

    Object.keys(data).forEach(key => {
      result.push(new DiceModel(data[key], DiceType.XocXoc));
    });

    return result;
  }

  getWinner() {
    const { result } = this.state;
    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);

    let winner = '';
    switch (sumDicesValue) {
      case 0:
        winner = 'white4';
        break;
      case 1:
        winner = 'white3';
        break;
      case 2:
        winner = 'draw';
        break;
      case 3:
        winner = 'red3';
        break;
      case 4:
        winner = 'red4';
        break;
    }

    return winner;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;
    const winer = gameState == GameState.waiting ? this.getWinner() : '';

    return (
      <Container className={styles['shakeThePlate-container']}>
        <Row>
          <Col align='center' className={styles['shakeThePlate-timmer']}>
            <TimeCounter remainingTime={time.remainingTime} gameState={gameState} />
            <div>
              <div className={styles.title}>Mini Se Die</div>
              <Row className={styles.result}>
                <Dice diceModel={result[0]} isSmall={false} />
                <Dice diceModel={result[1]} isSmall={false} />
                <Dice diceModel={result[2]} isSmall={false} />
                <Dice diceModel={result[3]} isSmall={false} />
              </Row>
              <RoundNum roundNumber={roundNumber} />
            </div>
          </Col>
          <Col align='center' className={styles['shakeThePlate-betList']}>
            <Col className={styles['col-plateeven']}>
              <TotalBet type='plateeven' total={betList.even.total} isWin={winer != '' && winer != 'white3' && winer != 'red3'} />
            </Col>
            <Col className={styles['col-plateodd']}>
              <TotalBet type='plateodd' total={betList.odd.total} isWin={winer == 'white3' || winer == 'red3'} />
            </Col>
            <Col>
              <TotalBet type='bondo' total={betList.red4.total} isWin={winer == 'red4'} />
            </Col>
            <Col>
              <TotalBet type='mottrangbado' total={betList.red3.total} isWin={winer == 'red3'} />
            </Col>
            <Col>
              <TotalBet type='motdobatrang' total={betList.white3.total} isWin={winer == 'white3'} />
            </Col>
            <Col>
              <TotalBet type='bontrang' total={betList.white4.total} isWin={winer == 'white4'} />
            </Col>
          </Col>
        </Row>
      </Container>
    )
  }

  renderBetList() {
    const { betList, gameState } = this.state;
    const winer = gameState == GameState.waiting ? this.getWinner() : '';

    return (
      <React.Fragment>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.bondo}></div>)]}
            type='simple'
            dataList={betList.red4.dataList}
            isWin={winer == 'red4'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.mottrangbado}></div>)]}
            type='simple'
            dataList={betList.red3.dataList}
            isWin={winer == 'red3'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles['title-chan']}>Even</div>)]}
            type='simple'
            dataList={betList.even.dataList}
            isWin={winer != '' && winer != 'white3' && winer != 'red3'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles['title-le']}>Odd</div>)]}
            type='simple'
            dataList={betList.odd.dataList}
            isWin={winer == 'white3' || winer == 'red3'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.motdobatrang}></div>)]}
            type='simple'
            dataList={betList.white3.dataList}
            isWin={winer == 'white3'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.bontrang}></div>)]}
            type='simple'
            dataList={betList.white4.dataList}
            isWin={winer == 'white4'}
          />
        </Col>
      </React.Fragment>
    )
  }

  render() {
    return (
      <div>
        <GameTemplate renderBetList={this.renderBetList()} renderMonitor={this.renderMonitor()} disabled={this.state.disabled}></GameTemplate>
      </div>
    );
  }
}
