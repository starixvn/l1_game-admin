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
      const currentTime = this.state.time.remainingTime;

      if (currentTime > 0) {
        this.setState({
          time: {
            remainingTime: currentTime - 1
          },
          betList: this.getTotalBetList(data.info.red, this.state.betList)
        });
      }
      else {
        this.setState({
          betList: this.getTotalBetList(data.info.red, this.state.betList)
        })
      }
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

    if (!!data.finish) {
      this.setState({
        result: this.getResult(data.finish),
        time: {
          remainingTime: 0
        }
      });

      let waitingTime = 6;

      const interval = setInterval(() => {
        if (waitingTime > 0) {
          waitingTime--;
          this.setState({
            time: {
              remainingTime: waitingTime
            }
          })
        }
        else {
          clearInterval(interval);

          this.setState({
            result: [
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc),
              new DiceModel(-1, DiceType.XocXoc)
            ],
            roundNumber: this.state.roundNumber + 1,
            time: {
              remainingTime: 30
            }
          })
        }
      }, 1000)
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
              currentData[key].dataList.push(new BetItem(data[user].name, data[user][key]))
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

  renderMonitor() {
    const { betList, roundNumber, time, result } = this.state;

    return (
      <Container className={styles['shakeThePlate-container']}>
        <Row>
          <Col align='center' className={styles['shakeThePlate-timmer']}>
            <TimeCounter remainingTime={time.remainingTime} />
            <Row className={styles.result}>
              <Dice diceModel={result[0]} isSmall={false} />
              <Dice diceModel={result[1]} isSmall={false} />
              <Dice diceModel={result[2]} isSmall={false} />
              <Dice diceModel={result[3]} isSmall={false} />
            </Row>
            <RoundNum roundNumber={roundNumber} />
          </Col>
          <Col align='center' className={styles['shakeThePlate-betList']}>
            <Col className={styles['col-plateeven']}>
              <TotalBet type='plateeven' total={betList.chan.total} />
            </Col>
            <Col className={styles['col-plateodd']}>
              <TotalBet type='plateodd' total={betList.le.total} />
            </Col>
            <Col>
              <TotalBet type='bondo' total={betList.red4.total} />
            </Col>
            <Col>
              <TotalBet type='mottrangbado' total={betList.red3.total} />
            </Col>
            <Col>
              <TotalBet type='motdobatrang' total={betList.white3.total} />
            </Col>
            <Col>
              <TotalBet type='bontrang' total={betList.white4.total} />
            </Col>

          </Col>
        </Row>
      </Container>
    )
  }

  renderBetList() {
    const { betList } = this.state;

    return (
      <React.Fragment>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.bondo}></div>)]}
            type='simple'
            dataList={betList.red4.dataList}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.mottrangbado}></div>)]}
            type='simple'
            dataList={betList.red3.dataList}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles['title-chan']}>Even</div>)]}
            type='simple'
            dataList={betList.chan.dataList}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles['title-le']}>Odd</div>)]}
            type='simple'
            dataList={betList.le.dataList}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.motdobatrang}></div>)]}
            type='simple'
            dataList={betList.white3.dataList}
          />
        </Col>
        <Col className='col-4 col-lg-2'>
          <BetList
            titles={[(<div className={styles.bontrang}></div>)]}
            type='simple'
            dataList={betList.white4.dataList}
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
