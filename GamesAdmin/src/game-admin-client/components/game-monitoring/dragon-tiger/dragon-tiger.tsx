import React from "react";
import TimeCounter from "../../shared/time-counter/time-counter";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import { Container, Row, Col } from "reactstrap";
import RoundNum from "../../shared/round-num/round-num";
import DragonTigerResult from "../../shared/dragon-tiger-result/dragon-tiger-result";
import styles from "./dragon-tiger.module.scss";
import CardResult from "../../common/model/CardResult";
import DragonTigerModel from "../../common/model/DragonTigerModel";
import BetItem from "../../common/model/BetItem";
import { upperFirstLetter } from "../../util/util";

type Props = {
  onMessage: Function;
};

type State = {
  data: DragonTigerModel;
  finished: boolean;
  isRunning: boolean;
};

const TYPES = new Array("dragon", "tie", "tiger");
const TOTAL_TIME = 30;
const WAITING_TIME = 5;

export default class DragonTiger extends React.Component<Props, State> {
  constructor(props: any) {
    super(props);

    this.state = {
      data: new DragonTigerModel(),
      finished: false,
      isRunning: false,
    };
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleMessage(message));
  }

  handleMessage(message) {
    if (message.phien !== undefined) {
      this.handleStart(message);
    } else if (message.finish !== undefined) {
      this.handleStop(message);
    } else {
      this.handleBet(message);
    }
  }

  getBet(type, userObj) {
    const betItem = new BetItem();
    betItem.name = userObj.name;
    betItem.bet = userObj[type];

    return betItem;
  }

  getCardResult(resultObj) {
    const cardResult = new CardResult();
    cardResult.card = resultObj.card;
    cardResult.type = resultObj.type;

    return cardResult;
  }

  waitingCounter(_this) {
    const _timeCounter = setInterval(function () {
      if (!_this.state.isRunning && _this.state.data.time.remainingTime > 0) {
        const _newData = _this.state.data;
        _newData.time.remainingTime = _newData.time.remainingTime - 1;
        _this.setState({
          data: _newData,
          finished: true,
        });
      } else {
        clearInterval(_timeCounter);
      }
    }, 1000);
  }

  handleBet(message) {
    const newData = new DragonTigerModel();
    newData.roundNumber = this.state.data.roundNumber;

    TYPES.forEach((type) => {
      newData[type].total = message.info[type];
    });

    const _this = this;
    Object.keys(message.ingame).forEach(function (key) {
      const userObj = message.ingame[key];
      TYPES.forEach((type) => {
        if (userObj[type] > 0) {
          newData[type].dataList.push(_this.getBet(type, userObj));
        }
      });
    });

    if (this.state.finished) {
      newData.roundNumber += 1;
      newData.time.remainingTime = TOTAL_TIME - 1;
      this.setState({
        data: newData,
        finished: false,
        isRunning: true,
      });
    } else {
      if (this.state.data.time.remainingTime > 0) {
        newData.time.remainingTime = this.state.data.time.remainingTime - 1;
      }

      this.setState({
        data: newData,
        isRunning: true,
      });
    }
  }

  handleStart(message) {
    const newData = new DragonTigerModel();
    newData.roundNumber = message.phien;

    if (message.time_remain > TOTAL_TIME) {
      newData.time.remainingTime = message.time_remain - TOTAL_TIME;
      const _this = this;
      this.waitingCounter(_this);
    } else {
      newData.time.remainingTime = message.time_remain;
    }

    const result = message.result;
    newData.roundNumber = message.phien;
    newData.result = new Array(
      this.getCardResult(result[0]),
      this.getCardResult(result[1])
    );
    this.setState({
      data: newData,
    });
  }

  handleStop(message) {
    const newData = this.state.data;
    const result = message.finish.result;
    newData.roundNumber = message.finish.round;
    newData.result = new Array(
      this.getCardResult(result[0]),
      this.getCardResult(result[1])
    );
    if (newData.time.remainingTime > 0) {
      newData.time.remainingTime = newData.time.remainingTime - 1;
    }

    this.setState({
      data: newData,
      finished: true,
      isRunning: false,
    });

    const _this = this;
    setTimeout(function () {
      const newData = _this.state.data;
      newData.time.remainingTime = WAITING_TIME;
      _this.setState(
        {
          data: newData,
        },
        () => _this.waitingCounter(_this)
      );
    }, 1000);
  }

  renderMonitor() {
    return (
      <Container className={styles["dragonTiger-container"]}>
        <Row>
          <div className={styles.counter}>
            <TimeCounter remainingTime={this.state.data.time.remainingTime} />
            <RoundNum roundNumber={this.state.data.roundNumber} />
          </div>
          <Col align="center">
            <TotalBet type="dragon" total={this.state.data.dragon.total} />
          </Col>

          <Col align="center">
            <Row>
              <Col>
                <DragonTigerResult result={this.state.data.result} />
              </Col>
            </Row>
            <Row>
              <Col className={styles.draw}>
                <TotalBet type="draw" total={this.state.data.tie.total} />
              </Col>
            </Row>
          </Col>

          <Col align="center">
            <TotalBet type="tiger" total={this.state.data.tiger.total} />
          </Col>
        </Row>
      </Container>
    );
  }

  renderBetList() {
    const result = [];
    const _this = this;
    TYPES.forEach(function (type, i) {
      result.push(
        <Col className="p-1" key={i}>
          <BetList
            type="simple"
            titles={[_this.renderTitle(upperFirstLetter(type))]}
            dataList={_this.state.data[type].dataList}
          />
        </Col>
      );
    });

    return (
      <Container>
        <Row>{result}</Row>
      </Container>
    );
  }

  renderTitle(title) {
    return <div>{title}</div>;
  }

  render() {
    return (
      <div className={styles.dragontiger}>
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
        ></GameTemplate>
      </div>
    );
  }
}
