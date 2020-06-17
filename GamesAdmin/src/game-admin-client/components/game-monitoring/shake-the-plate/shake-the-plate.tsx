import React from 'react';
import styles from './shake-the-plate.module.scss';
import TimeCounter from '../../shared/time-counter/time-counter';
import TotalBet from '../../shared/total-bet/total-bet';
import { Col, Container, Row } from 'reactstrap';
import GameTemplate from '../game-template/game-template';
import BetList from '../../shared/bet-list/bet-list';
import Dice from '../../shared/dice/dice';
import DiceModel from '../../common/model/DiceModel';
import DiceType from '../../common/model/DiceType';
import BetItem from '../../common/model/BetItem';
import ShakeThePlaceModel from '../../common/model/ShakeThePlateModel';
import RoundNum from '../../shared/round-num/round-num';
import { GameState } from '../../shared/common/GameState';

type ShakeThePlateProps = {
  onMessage: Function
}

export default class ShakeThePlate extends React.Component<ShakeThePlateProps, ShakeThePlaceModel> {
  constructor(props) {
    super(props);

    this.state = new ShakeThePlaceModel();

  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    if (!!data.info) {
      this.setState({
        betList: this.getTotalBetList(data.info.red, this.state.betList)
      })
    }

    if (!!data.ingame) {
      this.setState({
        betList: this.getUserBetList(data.ingame.red, this.state.betList)
      })
    }

    if (!!data.phien) {
      this.setState({
        roundNumber: data.phien,
      });
    }

    if (!!data.time_remain) {
      if (data.time_remain > 30) {
        this.setState({
          gameState: GameState.waiting,
          time: {
            remainingTime: data.time_remain - 31
          }
        })

        setTimeout(() => {
          this.setState({
            gameState: GameState.running,
            roundNumber: this.state.roundNumber + 1,
            time: {
              remainingTime: 30
            }
          })
        }, (data.time_remain - 30) * 1000)
      }
      else {
        this.setState({
          gameState: GameState.running,
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: data.time_remain
          }
        })
      }
    }

    if (!!data.finish) {
      const waitingTime = 9;
      this.setState({
        result: this.getResult(data.finish),
        gameState: GameState.waiting,
        time: {
          remainingTime: waitingTime
        }
      });

      setTimeout(() => {
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
      }, waitingTime * 1000)
    }
  }

  getTotalBetList(data, currentData) {
    currentData.chan.total = data.chan;
    currentData.le.total = data.le;
    currentData.red3.total = data.red3;
    currentData.red4.total = data.red4;
    currentData.white3.total = data.white3;
    currentData.white4.total = data.white4;

    return currentData;
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach(key => {
      currentData[key].dataList = new Array<BetItem>();
    });

    for (const user in data) {
      Object.keys(data[user]).forEach(key => {
        switch (key) {
          case 'name':
            break;
          case 'cuocQueue':
            break;
          case 'currentCuoc':
            break;
          default:
            if (data[user][key] > 0)
              currentData[key].dataList.unshift(new BetItem(data[user].name, data[user][key]))
        }
      })
    };

    return currentData;
  }

  getResult(data) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(data.dots[0], DiceType.XocXoc));
    result.push(new DiceModel(data.dots[1], DiceType.XocXoc));
    result.push(new DiceModel(data.dots[2], DiceType.XocXoc));
    result.push(new DiceModel(data.dots[3], DiceType.XocXoc));

    return result;
  }

  getWiner() {
    const { result } = this.state;
    const sumDicesValue = result.reduce((sum, dice) => sum + dice.value, 0);

    let winer = '';
    switch (sumDicesValue) {
      case 0:
        winer = 'white4';
        break;
      case 1:
        winer = 'white3';
        break;
      case 2:
        winer = 'draw';
        break;
      case 3:
        winer = 'red3';
        break;
      case 4:
        winer = 'red4';
        break;
    }

    return winer;
  }

  renderMonitor() {
    const { betList, roundNumber, time, result, gameState } = this.state;
    const winer = gameState == GameState.waiting ? this.getWiner() : '';

    return (
      <Container className={styles['shakeThePlate-container']}>
        <Row>
          <Col align='center' className={styles['shakeThePlate-timmer']}>
            <TimeCounter remainingTime={time.remainingTime} gameState={gameState} key={gameState} />
            <div>
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
            <Col className={styles['col-plateodd']}>
              <TotalBet type='plateodd' total={betList.le.total} isWin={winer == 'white3' || winer == 'red3'} />
            </Col>
            <Col className={styles['col-plateeven']}>
              <TotalBet type='plateeven' total={betList.chan.total} isWin={winer != '' && winer != 'white3' && winer != 'red3'} />
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
    const winer = gameState == GameState.waiting ? this.getWiner() : '';

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
            titles={[(<div className={styles['title-le']}>Odd</div>)]}
            type='simple'
            dataList={betList.le.dataList}
            isWin={winer == 'white3' || winer == 'red3'}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles['title-chan']}>Even</div>)]}
            type='simple'
            dataList={betList.chan.dataList}
            isWin={winer != '' && winer != 'white3' && winer != 'red3'}
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
        <GameTemplate renderBetList={this.renderBetList()} renderMonitor={this.renderMonitor()}></GameTemplate>
      </div>
    );
  }
}
