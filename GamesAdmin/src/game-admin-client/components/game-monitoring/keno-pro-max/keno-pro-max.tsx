import React from 'react';
import styles from './keno-pro-max.module.scss';
import TimeCounter from '../../shared/time-counter/time-counter';
import TotalBet from '../../shared/total-bet/total-bet';
import { Col, Container, Row } from 'reactstrap';
import GameTemplate from '../game-template/game-template';
import BetList from '../../shared/bet-list/bet-list';
import KennoResult from '../../shared/keno-result/keno-result';
import RoundNum from '../../shared/round-num/round-num';
import KenoProMaxModel from '../../common/model/KenoProMaxModel';
import BetItem from '../../common/model/BetItem';
import { upperFirstLetter } from '../../util/util';
import KenoResultUpdater from '../../shared/keno-result-updater/keno-result-updater';

type Props = {
  onMessage: Function,
  sendMessage: Function
};

type State = {
  data: KenoProMaxModel,
  finished: boolean,
  isRunning: boolean,
  isShowResultUpdater: boolean
};

const TYPES = new Array("big", "small", "even", "odd", "earth", "fire", "gold", "water", "wood");
const TOTAL_TIME = 30;
const WAITING_TIME = 8;

export default class KenoProMax extends React.Component<Props, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      data: new KenoProMaxModel(),
      finished: false,
      isRunning: false,
      isShowResultUpdater: false
    }

    this.showUpdater = this.showUpdater.bind(this);
    this.updaterToggle = this.updaterToggle.bind(this);
    this.updaterSubmit = this.updaterSubmit.bind(this);
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleMessage(message));
  }

  handleMessage(message) {
    if (message.remainingTime !== undefined) {
      this.handleStart(message);
    }
    else if (message.finish !== undefined) {
      this.handleStop(message);
    }
    else {
      this.handleBet(message);
    }
  }

  getSettlementResult(result) {
    const resultArr = [];
    TYPES.forEach(function (type) {
      if (result[type]) {
        resultArr.push(type.toUpperCase());
      }
    })

    return resultArr.join(" - ");
  }

  handleStart(message) {
    const newData = new KenoProMaxModel();
    newData.roundNumber = message.roundId;

    if (message.remainingTime > TOTAL_TIME) {
      newData.time.remainingTime = message.remainingTime - TOTAL_TIME;
      const _this = this;
      this.waitingCounter(_this);
    }
    else {
      newData.time.remainingTime = message.remainingTime;
    }

    this.setState({
      data: newData
    })
  }

  waitingCounter(_this) {
    const _timeCounter = setInterval(function () {
      if (!_this.state.isRunning && _this.state.data.time.remainingTime > 0) {
        const _newData = _this.state.data;
        _newData.time.remainingTime = _newData.time.remainingTime - 1;
        _this.setState({
          data: _newData,
          finished: true
        })
      }
      else {
        clearInterval(_timeCounter);
      }
    }, 1000);
  }

  handleStop(message) {
    const newData = this.state.data;
    const result = message.finish.result;
    newData.roundNumber = message.finish.roundId;
    newData.result = result;
    newData.settlementResult = this.getSettlementResult(message.finish.settlementResult);
    newData.resultSumText = result.reduce((sum, x) => sum + x, 0)

    if (newData.time.remainingTime > 0) {
      newData.time.remainingTime = newData.time.remainingTime - 1;
    }

    this.setState({
      data: newData,
      finished: true,
      isRunning: false
    })

    const _this = this;
    setTimeout(function () {
      const newData = _this.state.data;
      newData.time.remainingTime = WAITING_TIME;
      _this.setState({
        data: newData
      }, () => _this.waitingCounter(_this))
    }, 1000)
  }

  handleBet(message) {
    const _this = this;
    const newData = new KenoProMaxModel();
    newData.roundNumber = this.state.data.roundNumber;

    Object.keys(message.ingame).forEach(function (key) {
      const userObj = message.ingame[key];
      TYPES.forEach(type => {
        if (userObj[type] > 0) {
          newData[type].dataList.push(_this.getBet(type, userObj));
        }
      });
    });

    TYPES.forEach(type => {
      newData[type].total = newData[type].dataList.reduce((sum, x) => sum + x.bet, 0);
    });

    if (this.state.finished) {
      newData.roundNumber += 1;
      newData.time.remainingTime = TOTAL_TIME - 1;
      this.setState({
        data: newData,
        finished: false,
        isRunning: true
      })
    }
    else {
      if (this.state.data.time.remainingTime > 0) {
        newData.time.remainingTime = this.state.data.time.remainingTime - 1;
      }

      this.setState({
        data: newData,
        isRunning: true
      })
    }
  }

  getBet(type, userObj) {
    const betItem = new BetItem();
    betItem.name = userObj.name;
    betItem.bet = userObj[type];

    return betItem;
  }

  showUpdater() {
    this.setState({
      isShowResultUpdater: true
    })
  }

  updaterToggle() {
    this.setState({
      isShowResultUpdater: !this.state.isShowResultUpdater
    })
  }

  updaterSubmit(list) {
    const obj = {
      setResult: {
        result: list,
        roundId: this.state.isRunning ? this.state.data.roundNumber : this.state.data.roundNumber + 1
      }
    }

    this.setState({
      isShowResultUpdater: false
    })
    this.props.sendMessage(obj);
  }

  renderMonitor() {
    return (
      <Container>
        <Row>
          <Col className='col-12 col-lg-4'>
            <Row className='justify-content-center'>
              <div className={styles.timecounter}>
                <TimeCounter remainingTime={this.state.data.time.remainingTime}></TimeCounter>
                <RoundNum roundNumber={this.state.data.roundNumber} />
                <div className={styles['result-sum']}>
                  {this.state.data.resultSumText}
                </div>
              </div>
              <div onClick={this.showUpdater}>
                <KennoResult listResult={this.state.data.result}></KennoResult>
                <div className={styles['setttlement-result']}>
                  {this.state.data.settlementResult}
                </div>
              </div>
            </Row>
          </Col>
          <Col className='p-0 col-12 col-lg-8'>
            <Row className={styles['row-style']}>
              <TotalBet type='kenogold' total={this.state.data.gold.total} />
              <TotalBet type='kenowood' total={this.state.data.wood.total} />
              <TotalBet type='kenowater' total={this.state.data.water.total} />
              <TotalBet type='kenofire' total={this.state.data.fire.total} />
              <TotalBet type='kenoearth' total={this.state.data.earth.total} />
            </Row>
            <Row className={styles['row-style']}>
              <TotalBet type='kenobig' total={this.state.data.big.total} />
              <TotalBet type='kenosmall' total={this.state.data.small.total} />
              <TotalBet type='kenoeven' total={this.state.data.even.total} />
              <TotalBet type='kenoodd' total={this.state.data.odd.total} />
            </Row>
          </Col>
        </Row>
      </Container>
    )
  }

  renderBetList() {
    const result = [];
    const _this = this;
    TYPES.forEach(function (type, i) {
      result.push((
        <Col className={styles['keno-betlist']} key={i}>
          <BetList type="onecolumn" titles={[<div>{_this.renderTitle(upperFirstLetter(type))}</div>]} dataList={_this.state.data[type].dataList} />
        </Col>
      ));
    })

    return (
      <React.Fragment>
        {result}
      </React.Fragment>
    )
  }

  renderTitle(value) {
    return <span className={styles['keno-title']}>{value}</span>
  }

  render() {
    return (
      <div>
        <GameTemplate renderMonitor={this.renderMonitor()} renderBetList={this.renderBetList()} />
        <KenoResultUpdater modal={this.state.isShowResultUpdater} toggle={this.updaterToggle} submit={this.updaterSubmit} />
      </div>
    );
  }
}
