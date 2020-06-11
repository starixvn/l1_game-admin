import React from "react";
import styles from "./big-small-base.module.scss";
import TimeCounter from "../../shared/time-counter/time-counter";
import DiceResult from "../../shared/dice-result/dice-result";
import TotalBet from "../../shared/total-bet/total-bet";
import GameTemplate from "../game-template/game-template";
import BetList from "../../shared/bet-list/bet-list";
import { Container, Row, Col } from "reactstrap";
import TotalUser from "../../shared/total-user/total-user";
import RoundNum from "../../shared/round-num/round-num";
import DiceModel from "../../common/model/DiceModel";
import BigSmallBaseModel from '../../common/model/BigSmallBaseModel';
import DiceType from '../../common/model/DiceType';
import BetItem from '../../common/model/BetItem';

const types = {
  taixiu: "taixiu",
  chanle: "chanle",
  clturbo: "clturbo"
};

const gameMapping = {
  taixiu: ['tai', 'xiu'],
  chanle: ['chan', 'le'],
  clturbo: ['chan', 'le']
}

type BigSmalllProps = {
  turbo: boolean,
  type: string,
  onMessage: Function
}

export default class BigSmallBase extends React.Component<BigSmalllProps, BigSmallBaseModel> {
  constructor(props) {
    super(props);

    this.state = new BigSmallBaseModel();
  }

  componentDidMount() {
    this.props.onMessage((message) => this.handleData(message));
  }

  handleData(data) {
    const { turbo, type } = this.props;
    const totalRemainingTime = this.props.turbo ? 30 : 50;
    const totalWaitingThime = this.props.turbo ? 5 : 15;
    const gameType = turbo && type == 'chanle' ? 'clturbo' : type;

    if (data.taixiu != null || data.chanle != null || data.clturbo != null) {
      const currentTime = this.state.time.remainingTime;

      if (currentTime > 0) {
        this.setState({
          time: {
            remainingTime: currentTime - 1
          },
          betList: this.getTotalBetList(data[gameType], this.state.betList, gameType),
          firstBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][0]}`],
          secondBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][1]}`]
        });
      }
      else {
        this.setState({
          betList: this.getTotalBetList(data[gameType], this.state.betList, gameType),
          firstBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][0]}`],
          secondBetTypeUsers: data[gameType][`red_player_${gameMapping[gameType][1]}`]
        })
      }
    }

    if (!!data.phien) {
      this.setState({
        roundNumber: data.phien,
      });
    }

    if (!!data.list) {
      this.setState({
        betList: this.getUserBetList(data.list, this.state.betList)
      })
    }

    if (!!data.finish) {
      this.setState({
        result: this.getResult(data.finish),
        time: {
          remainingTime: 0
        }
      });

      this.countDownWaitingTime(totalWaitingThime + 1, () => {
        this.setState({
          result: [
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu)
          ],
          roundNumber: this.state.roundNumber + 1,
          time: {
            remainingTime: totalRemainingTime + 1
          }
        })
      })
    }

    if (!!data.time_remain) {
      if ((this.props.turbo && data.time_remain > totalRemainingTime + 6) || data.time_remain > totalRemainingTime) {
        this.countDownWaitingTime(data.time_remain - totalRemainingTime, () => {
          this.setState({
            time: {
              remainingTime: totalRemainingTime,
            },
            result: [
              new DiceModel(-1, DiceType.TaiXiu),
              new DiceModel(-1, DiceType.TaiXiu),
              new DiceModel(-1, DiceType.TaiXiu)
            ]
          })
        })
      } else {
        this.setState({
          time: {
            remainingTime: data.time_remain
          },
          result: [
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu),
            new DiceModel(-1, DiceType.TaiXiu)
          ]
        })
      }
    }
  }

  countDownWaitingTime(waitingTime, callBack) {
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
        callBack();
      }
    }, 1000);
  }

  getTotalBetList(data, currentData, gameType) {
    currentData.firstBetType.total = data[`red_${gameMapping[gameType][0]}`];
    currentData.secondBetType.total = data[`red_${gameMapping[gameType][1]}`];

    return currentData;
  }

  getUserBetList(data, currentData) {
    Object.keys(currentData).forEach(key => {
      currentData[key].dataList = new Array<BetItem>();
    });

    data.forEach(user => {
      if (user.select == true) {
        currentData.firstBetType.dataList.push(new BetItem(user.name, user.bet, user.time));
      }
      else {
        currentData.secondBetType.dataList.push(new BetItem(user.name, user.bet, user.time));
      }
    });

    return currentData;
  }

  getResult(data) {
    const result = new Array<DiceModel>();

    result.push(new DiceModel(data.dices[0], DiceType.TaiXiu));
    result.push(new DiceModel(data.dices[1], DiceType.TaiXiu));
    result.push(new DiceModel(data.dices[2], DiceType.TaiXiu));

    return result;
  }


  renderMonitor() {
    const { turbo, type } = this.props;
    const { betList, time, roundNumber, result, firstBetTypeUsers, secondBetTypeUsers } = this.state;

    let turboText = turbo ? "Turbo" : "";
    let type1 = type == types.taixiu ? "big" : "chan";
    let type2 = type == types.taixiu ? "small" : "le";
    return (
      <Container>
        <Row>
          <span className={styles.turbo}>
            <div>{turboText}</div>
          </span>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet type={type1} total={betList.firstBetType.total} />
            <TotalUser totalUser={firstBetTypeUsers} />
          </Col>
          <Col align="center" className={styles["base-result"]}>
            <DiceResult dices={result} />
            <TimeCounter remainingTime={time.remainingTime} />
            <RoundNum roundNumber={roundNumber} />
          </Col>
          <Col align="center" className={styles["flex-col"]}>
            <TotalBet type={type2} total={betList.secondBetType.total} />
            <TotalUser totalUser={secondBetTypeUsers} />
          </Col>
        </Row>
      </Container>
    );
  };

  renderBetList() {
    const { betList } = this.state;

    return (
      <Container>
        <Row className={styles["wrap-result"]}>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.firstBetType.dataList}
              type="withtime"
              titles={this.getTitles()}
            ></BetList>
          </Col>
          <Col className="col-12 col-sm-6">
            <BetList
              dataList={betList.secondBetType.dataList}
              type="withtime"
              titles={this.getTitles()}
            ></BetList>
          </Col>
        </Row>
      </Container>
    );
  };

  getTitles() {
    return [
      'Time',
      'Nickname',
      'Stake'
    ]
  };

  render() {
    return (
      <div className={styles.bigsmallturbo}>
        <GameTemplate
          renderBetList={this.renderBetList()}
          renderMonitor={this.renderMonitor()}
        />
      </div>
    );
  }
}
